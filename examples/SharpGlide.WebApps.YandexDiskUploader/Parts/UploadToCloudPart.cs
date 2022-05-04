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
            ResetInternalState();

            var exceptions = new List<Exception>();

            try
            {
                await WalkThroughFiles(cancellationToken);
                DetectFoldersForUpload(exceptions);
                await UploadFolders(cancellationToken, exceptions);
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        private async Task UploadFolders(CancellationToken cancellationToken, List<Exception> exceptions)
        {
            var foldersToCreate = _workingFilesPerFolder.Keys
                .Select(s => new UploaderWorkingFolder
                {
                    FullName = s
                }).ToArray();

            foreach (var folder in foldersToCreate)
            {
                try
                {
                    folder.CloudName = folder.FullName.ToCloudPath(_stateRoot.LocalFolder);
                    await _singleFolderCreator.WriteSingle(_stateRoot.SecuritySection, folder, Route.Default,
                        cancellationToken);

                    await ProcessFiles(_workingFilesPerFolder[folder.FullName], cancellationToken);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
        }

        private void DetectFoldersForUpload(List<Exception> exceptions)
        {
            var filesPerFolder = _workingFiles
                .GroupBy(g =>
                {
                    try
                    {
                        return Path.GetDirectoryName(((ICloudFileInformation)g).FullName);
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
                _workingFilesPerFolder[group.Key] = new List<UploaderWorkingFile>();
                _workingFilesPerFolder[group.Key].AddRange(group.ToList());
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
                },  async (token, infos) =>
                {
                    var entryInfos = infos as FsEntryInfo[] ?? infos.ToArray();

                    _workingFiles.AddRange(entryInfos.Select(s => new UploaderWorkingFile(s)).ToArray());

                    _localFilesStatistic.FilesCount += entryInfos.Count();
                    _localFilesStatistic.TotalSize += entryInfos.Select(s => s.Size).Sum();
                    _localFilesStatistic.FoldersCount++;

                    var data = JsonSerializer.Serialize(_localFilesStatistic);
                    await _realtimeUpdatesHub.Clients.All.SendAsync(RealtimeUpdatesHub.ReceiveLocalFileSystemStatInfo, data,
                        token);
                });
        }

        private Task ProcessFiles(List<UploaderWorkingFile> uploaderWorkingFiles,
            CancellationToken cancellationToken)
        {
            var concurrentBag = new ConcurrentBag<Exception>();
            Parallel.For(0, uploaderWorkingFiles.Count, _parallelOptions, i =>
            {
                try
                {
                    var workingFile = uploaderWorkingFiles.ElementAt(i);
                    var workingFileToProcessOnCloud = _singleFileUploader.WriteAndReturnSingle(_stateRoot.SecuritySection,
                        workingFile,
                        Route.Default,
                        cancellationToken).GetAwaiter().GetResult();
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