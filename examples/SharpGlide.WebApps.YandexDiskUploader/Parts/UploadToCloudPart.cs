using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly ContentFlowStatistic _localFilesStatistic = new();
        private readonly ContentFlowStatistic _cloudFilesStatistic = new();

        private readonly List<UploaderWorkingFile> _workingFiles = new();
        private readonly List<UploaderWorkingFolder> _workingFolders = new();
        private readonly Dictionary<string, List<UploaderWorkingFile>> _workingFilesPerFolder = new();

        private readonly Stopwatch _stopWatch = new Stopwatch();
        private readonly List<long> _foldersUploadWatchMark = new List<long>();
        private  readonly List<long> _filesUploadWatchMark = new List<long>();
        private readonly List<Tuple<long, long>> _fileSizesUploadWatchMark = new List<Tuple<long, long>>();

        private readonly ParallelOptions _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 5
        };
            
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
                
                _stopWatch.Start();
                
                await WalkThroughFiles(cancellationToken);
                CalculatePath(exceptions);
                await UploadFolders(cancellationToken, exceptions);
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }
            finally
            {
                _stopWatch.Stop();
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
                    
                    _foldersUploadWatchMark.Add(_stopWatch.ElapsedMilliseconds);

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

                    if (entryInfos.Any())
                    {
                        _workingFiles.AddRange(entryInfos.Select(s => new UploaderWorkingFile(s)).ToArray());

                        _localFilesStatistic.FilesCount += entryInfos.Count();
                        _localFilesStatistic.TotalSize += entryInfos.Select(s => s.Size).Sum();
                        _localFilesStatistic.FoldersCount++;

                        await _realtimeUpdatesHub.SendLocalStatInfo(_localFilesStatistic, cancellationToken);
                    }
                });
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
                        _filesUploadWatchMark.Add(_stopWatch.ElapsedMilliseconds);
                        _fileSizesUploadWatchMark.Add(new Tuple<long, long>(_stopWatch.ElapsedMilliseconds, workingFile.Size));
                        
                        _cloudFilesStatistic.FilesCount++;
                        _cloudFilesStatistic.TotalSize += workingFile.Size;
                        
                        CalculateFlowStat();
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

        private void CalculateFlowStat()
        {
            try
            {
                _cloudFilesStatistic.SpeedFilesPerSec =
                    _filesUploadWatchMark.Count() / (_filesUploadWatchMark.Max() / 1000.0);
                _cloudFilesStatistic.SpeedMbPerSec = (_fileSizesUploadWatchMark.Sum(s => s.Item2)
                                                      / (_fileSizesUploadWatchMark.Max(s => s.Item1) / 1000.0)) /
                                                     1024.0 / 1024.0;
                _cloudFilesStatistic.TimeSpentSec = _stopWatch.ElapsedMilliseconds / 1000;

                if (_cloudFilesStatistic.SpeedFilesPerSec > 0)
                {
                    _cloudFilesStatistic.FinishInSec =
                        (long)Math.Round((_localFilesStatistic.FilesCount - _cloudFilesStatistic.FilesCount) /
                                   _cloudFilesStatistic.SpeedFilesPerSec, 0);
                }
                else
                {
                    _cloudFilesStatistic.FinishInSec = 0;
                }
            }
            catch
            {
                // ignored
            }
        }


        private void ResetInternalState()
        {
            _stopWatch.Reset();
            
            _filesUploadWatchMark.Clear();
            _foldersUploadWatchMark.Clear();
            
            _localFilesStatistic.FilesCount = 0;
            _localFilesStatistic.FoldersCount = 0;
            _localFilesStatistic.TotalSize = 0;

            _cloudFilesStatistic.FilesCount = 0;
            _cloudFilesStatistic.FoldersCount = 0;
            _cloudFilesStatistic.TotalSize = 0;

            _workingFolders.Clear();
            _workingFiles.Clear();
            _workingFilesPerFolder.Clear();
        }
    }
}