using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
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
    public class ContentSizeStatistic
    {
        public long FilesCount { get; set; }
        public long FoldersCount { get; set; }
        public long TotalSize { get; set; }
    }

    public class UploadToCloudPart : IUploadToCloudPart
    {
        private readonly IStateRoot _stateRoot;
        private readonly IHubContext<RealtimeUpdatesHub> _realtimeUpdatesHub;
        private readonly IFileSystemWalker _fileSystemWalker;
        private readonly ISingleFileUploader _singleFileUploader;
        public string Name { get; set; }

        private readonly ContentSizeStatistic _localFilesStatistic = new();
        private readonly ContentSizeStatistic _cloudFilesStatistic = new();

        private readonly List<UploaderWorkingFile> _listOfUploaderFiles = new();

        private readonly ParallelOptions _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 5
        };

        public UploadToCloudPart(
            IStateRoot stateRoot,
            IHubContext<RealtimeUpdatesHub> realtimeUpdatesHub,
            IFileSystemWalker fileSystemWalker,
            ISingleFileUploader singleFileUploader
        )
        {
            _stateRoot = stateRoot;
            _realtimeUpdatesHub = realtimeUpdatesHub;
            _fileSystemWalker = fileSystemWalker;
            _singleFileUploader = singleFileUploader;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            _localFilesStatistic.FilesCount = 0;
            _localFilesStatistic.FoldersCount = 0;
            _localFilesStatistic.TotalSize = 0;

            await _fileSystemWalker.WalkAsyncPagedAsync(cancellationToken, PageInfo.Default,
                new FsEntryInfo()
                {
                    FullName = _stateRoot.WorkingFolder,
                    Name = Path.GetFileName(_stateRoot.WorkingFolder),
                    Size = 0
                }, OnFileVisit);

            Parallel.For(0, _listOfUploaderFiles.Count, _parallelOptions, async l =>
            {
                var workingFile = _listOfUploaderFiles.ElementAt(l);

                var uploadedFile = await _singleFileUploader.WriteAndReturnSingle(_stateRoot.SecuritySection,
                    workingFile, Route.Default,
                    cancellationToken);
            });
        }

        private async Task OnFileVisit(CancellationToken cancellationToken, IEnumerable<FsEntryInfo> fsEntryInfos)
        {
            var entryInfos = fsEntryInfos as FsEntryInfo[] ?? fsEntryInfos.ToArray();

            _listOfUploaderFiles.AddRange(entryInfos.Select(s => new UploaderWorkingFile(s)).ToArray());

            _localFilesStatistic.FilesCount += entryInfos.Count();
            _localFilesStatistic.TotalSize += entryInfos.Select(s => s.Size).Sum();
            _localFilesStatistic.FoldersCount++;

            var data = JsonSerializer.Serialize(_localFilesStatistic);
            await _realtimeUpdatesHub.Clients.All.SendAsync(RealtimeUpdatesHub.ReceiveLocalFileSystemStatInfo, data,
                cancellationToken: cancellationToken);
        }
    }
}