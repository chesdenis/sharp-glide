using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SharpGlide.WebApps.YandexDiskUploader.Hubs
{
    public class RealtimeUpdatesHub : Hub
    {
        public const string HubEndpoint = "/realtime-updates-hub";
        
        public const string ReceiveLocalFileSystemStatInfo = nameof(ReceiveLocalFileSystemStatInfo);
        public async Task SendLocalFileSystemStatInfo(string data)
        {
            await Clients.All.SendAsync(ReceiveLocalFileSystemStatInfo, data);
        }

        public const string ReceiveCloudFileSystemStatInfo = nameof(ReceiveCloudFileSystemStatInfo);
        public async Task SendCloudFileSystemStatInfo(string data)
        {
            await Clients.All.SendAsync(nameof(ReceiveCloudFileSystemStatInfo), data);
        }
    }
}