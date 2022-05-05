using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SharpGlide.Cloud.Yandex.Extensions;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Cloud.Yandex.Writers.YandexDisc;
using SharpGlide.IO.Model;
using SharpGlide.IO.Readers;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Read.Model;
using SharpGlide.WebApps.YandexDiskUploader.Hubs;
using SharpGlide.WebApps.YandexDiskUploader.Model;
using SharpGlide.WebApps.YandexDiskUploader.State;

namespace SharpGlide.WebApps.YandexDiskUploader.Parts
{
    public class UploadToCloudPart : IUploadToCloudPart
    {
        private readonly IStateRoot _stateRoot;
        private readonly IHubContext<RealtimeUpdatesHub> _realtimeUpdatesHub;
        private readonly IFileSystemWalker _fileSystemWalker;
        private readonly IFileContentWalker _fileContentWalker;
        private readonly ISingleFileUploader _singleFileUploader;
        private readonly ISingleFolderCreator _singleFolderCreator;
        public string Name { get; set; }

        private readonly ContentSizeStatistic _localFilesStatistic = new();
        private readonly ContentSizeStatistic _cloudFilesStatistic = new();

        private readonly List<UploaderWorkingFile> _workingFiles = new();
        private readonly List<UploaderWorkingFolder> _workingFolders = new();
        private readonly Dictionary<string, List<UploaderWorkingFile>> _workingFilesPerFolder = new();

        private readonly ParallelOptions _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 5
        };

        private readonly PageInfo _defaultFileContentSize = new() { PageSize = 10000000 }; // 10Mb step

        public UploadToCloudPart(
            IStateRoot stateRoot,
            IHubContext<RealtimeUpdatesHub> realtimeUpdatesHub,
            IFileSystemWalker fileSystemWalker,
            IFileContentWalker fileContentWalker,
            ISingleFileUploader singleFileUploader,
            ISingleFolderCreator singleFolderCreator
        )
        {
            _stateRoot = stateRoot;
            _realtimeUpdatesHub = realtimeUpdatesHub;
            _fileSystemWalker = fileSystemWalker;
            _fileContentWalker = fileContentWalker;
            _singleFileUploader = singleFileUploader;
            _singleFolderCreator = singleFolderCreator;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            var exceptions = new List<Exception>();

            try
            {
                ResetInternalState();
                await WalkThroughFiles(cancellationToken);
                CalculatePath(exceptions);

                await UploadFolders(cancellationToken, exceptions);
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }

            if (exceptions.Count > 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, exceptions.Select((s => s.Message)).ToArray()));
            }
        }

        private async Task UploadFolders(CancellationToken cancellationToken, List<Exception> exceptions)
        {
            var rootFolder = new UploaderWorkingFolder()
            {
                Name = Path.GetFileName(_stateRoot.CloudFolder),
                FullName = _stateRoot.CloudFolder,
                CloudRelativePath = "/",
                CloudAbsolutePath = $"{_stateRoot.CloudFolder}/"
            };

            await _singleFolderCreator.WriteSingle(
                _stateRoot.SecurityTokens, rootFolder, Route.Default, cancellationToken);

            foreach (var folder in _workingFolders)
            {
                try
                {
                    await _singleFolderCreator.WriteSingle(_stateRoot.SecurityTokens, folder, Route.Default,
                        cancellationToken);

                    _cloudFilesStatistic.FoldersCount++;

                    await _realtimeUpdatesHub.SendCloudStatInfo(_cloudFilesStatistic, cancellationToken);
                }
                catch (Exception e)
                {
                    await _realtimeUpdatesHub.SendException(e.Message,
                        cancellationToken);
                    exceptions.Add(e);
                }

                try
                {
                    await UploadFiles(_workingFilesPerFolder[folder.FullName], cancellationToken);
                }
                catch (Exception e)
                {
                    await _realtimeUpdatesHub.SendException(e.Message,
                        cancellationToken);
                    exceptions.Add(e);
                }
            }
        }

        private void CalculatePath(List<Exception> exceptions)
        {
            var filesPerFolder = _workingFiles
                .GroupBy(g =>
                {
                    try
                    {
                        return Path.GetDirectoryName(g.FullName);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                        return string.Empty;
                    }
                })
                .Where(w => w.Key != null)
                .ToArray();

            foreach (var group in filesPerFolder)
            {
                foreach (var file in group.ToArray())
                {
                    file.RelativePath = file.FullName.CalculateRelativePath(_stateRoot.LocalFolder);
                    file.CloudRelativePath = file.RelativePath;
                    file.CloudAbsolutePath = _stateRoot.CloudFolder + file.CloudRelativePath;
                }

                _workingFilesPerFolder[group.Key] = new List<UploaderWorkingFile>();
                _workingFilesPerFolder[group.Key].AddRange(group.ToArray());

                _workingFolders.Add(new UploaderWorkingFolder
                {
                    Name = Path.GetFileName(group.Key),
                    FullName = group.Key,
                    CloudRelativePath = group.Key.CalculateRelativePath(_stateRoot.LocalFolder),
                    CloudAbsolutePath = _stateRoot.CloudFolder +
                                        group.Key.CalculateRelativePath(_stateRoot.LocalFolder)
                });
            }
        }

        private async Task WalkThroughFiles(CancellationToken cancellationToken)
        {
            await _fileSystemWalker.WalkAsyncPagedAsync(cancellationToken, PageInfo.Default,
                new FsEntryInfo()
                {
                    FullName = _stateRoot.LocalFolder,
                    Name = Path.GetFileName(_stateRoot.LocalFolder),
                    Size = 0
                }, async (token, infos) =>
                {
                    var entryInfos = infos as FsEntryInfo[] ?? infos.ToArray();

                    _workingFiles.AddRange(entryInfos.Select(s => new UploaderWorkingFile(s)).ToArray());

                    _localFilesStatistic.FilesCount += entryInfos.Count();
                    _localFilesStatistic.TotalSize += entryInfos.Select(s => s.Size).Sum();
                    _localFilesStatistic.FoldersCount++;

                    await _realtimeUpdatesHub.SendLocalStatInfo(_localFilesStatistic, cancellationToken);
                });
        }

        private Task UploadFiles(List<UploaderWorkingFile> uploaderWorkingFiles,
            CancellationToken cancellationToken)
        {
            var concurrentBag = new ConcurrentBag<Exception>();
            Parallel.For(0, uploaderWorkingFiles.Count, _parallelOptions, i =>
            {
                var workingFile = uploaderWorkingFiles.ElementAt(i);
                try
                {
                    var workingFileToProcessOnCloud = _singleFileUploader.WriteAndReturnSingle(
                        _stateRoot.SecurityTokens,
                        workingFile,
                        Route.Default,
                        cancellationToken).GetAwaiter().GetResult();

                    lock (_cloudFilesStatistic)
                    {
                        _cloudFilesStatistic.FilesCount++;
                        _cloudFilesStatistic.TotalSize += workingFile.Size;
                    }

                    _realtimeUpdatesHub.SendCloudStatInfo(_cloudFilesStatistic, cancellationToken)
                        .Wait(cancellationToken);
                }
                catch (Exception e)
                {
                    concurrentBag.Add(e);
                }
            });

            if (concurrentBag.Count > 0)
            {
                throw new AggregateException(concurrentBag.ToList());
            }

            return Task.CompletedTask;
        }


        private void ResetInternalState()
        {
            _localFilesStatistic.FilesCount = 0;
            _localFilesStatistic.FoldersCount = 0;
            _localFilesStatistic.TotalSize = 0;

            _cloudFilesStatistic.FilesCount = 0;
            _cloudFilesStatistic.FoldersCount = 0;
            _cloudFilesStatistic.TotalSize = 0;

            _workingFiles.Clear();
            _workingFilesPerFolder.Clear();
        }
    }
}