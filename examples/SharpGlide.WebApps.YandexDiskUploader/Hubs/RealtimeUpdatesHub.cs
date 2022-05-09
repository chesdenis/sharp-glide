using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SharpGlide.WebApps.YandexDiskUploader.Hubs
{
    public class RealtimeUpdatesHub : Hub
    {
        public const string HubEndpoint = "/realtime-updates-hub";

        public const string ReceiveLocalFileSystemStatInfo = nameof(ReceiveLocalFileSystemStatInfo);

        public const string ReceiveCloudFileSystemStatInfo = nameof(ReceiveCloudFileSystemStatInfo);

        public const string ReceiveExceptionOrConflicts = nameof(ReceiveExceptionOrConflicts);
    }
}