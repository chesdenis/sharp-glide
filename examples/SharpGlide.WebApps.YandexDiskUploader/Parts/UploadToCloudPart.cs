using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SharpGlide.IO.Model;
using SharpGlide.IO.Readers;
using SharpGlide.Tunnels.Read.Model;
using SharpGlide.WebApps.YandexDiskUploader.Config;
using SharpGlide.WebApps.YandexDiskUploader.Hubs;
using SharpGlide.WebApps.YandexDiskUploader.Model;
using SharpGlide.WebApps.YandexDiskUploader.State;
using FsEntryInfo = SharpGlide.IO.Model.FsEntryInfo;

namespace SharpGlide.WebApps.YandexDiskUploader.Parts
{
    public class UploadToCloudPart : IUploadToCloudPart
    {
        private readonly IStateRoot _stateRoot;
        private readonly IHubContext<RealtimeUpdatesHub> _realtimeUpdatesHub;
        private readonly IFileSystemWalker _fileSystemWalker;
        public string Name { get; set; }
        
        public class ContentSizeStatistic
        {
            public long FilesCount { get; set; }
            public long FoldersCount { get; set; }
            public long TotalSize { get; set; }
        }

        private readonly ContentSizeStatistic _sizeStatistic = new ContentSizeStatistic();

        public UploadToCloudPart(
            IStateRoot stateRoot,
            IHubContext<RealtimeUpdatesHub> realtimeUpdatesHub, 
            IFileSystemWalker fileSystemWalker)
        {
            _stateRoot = stateRoot;
            _realtimeUpdatesHub = realtimeUpdatesHub;
            _fileSystemWalker = fileSystemWalker;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            _sizeStatistic.FilesCount = 0;
            _sizeStatistic.FoldersCount = 0;
            _sizeStatistic.TotalSize = 0;
            
            await _fileSystemWalker.WalkAsyncPagedAsync(cancellationToken, PageInfo.Default, 
                new FsEntryInfo()
                {
                    FullName = _stateRoot.WorkingFolder,
                    Name = Path.GetFileName(_stateRoot.WorkingFolder),
                    Size = 0
                }, OnFileVisit);
        }

        private async Task OnFileVisit(CancellationToken cancellationToken, IEnumerable<FsEntryInfo> fsEntryInfos)
        {
            var entryInfos = fsEntryInfos as FsEntryInfo[] ?? fsEntryInfos.ToArray();
            
            _sizeStatistic.FilesCount += entryInfos.Count();
            _sizeStatistic.TotalSize += entryInfos.Select(s => s.Size).Sum();
            _sizeStatistic.FoldersCount++;

            var data = JsonSerializer.Serialize(_sizeStatistic);
            await _realtimeUpdatesHub.Clients.All.SendAsync(RealtimeUpdatesHub.ReceiveLocalFileSystemStatInfo, data,
                cancellationToken: cancellationToken);
        }
    }
}