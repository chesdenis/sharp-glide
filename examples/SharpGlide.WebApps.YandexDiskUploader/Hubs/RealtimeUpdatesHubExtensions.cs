using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SharpGlide.WebApps.YandexDiskUploader.Hubs
{
    public static class RealtimeUpdatesHubExtensions
    {
        public static async Task SendCloudStatInfo<T>(
            this IHubContext<RealtimeUpdatesHub> hub,
            T data,
            CancellationToken cancellationToken)
        {
            await hub.SendTo(data, RealtimeUpdatesHub.ReceiveCloudFileSystemStatInfo, cancellationToken);
        }

        public static async Task SendLocalStatInfo<T>(
            this IHubContext<RealtimeUpdatesHub> hub,
            T data,
            CancellationToken cancellationToken)
        {
            await hub.SendTo(data, RealtimeUpdatesHub.ReceiveLocalFileSystemStatInfo, cancellationToken);
        }
        
        public static async Task SendException<T>(
            this IHubContext<RealtimeUpdatesHub> hub,
            T data,
            CancellationToken cancellationToken)
        {
            await hub.SendTo(data, RealtimeUpdatesHub.ReceiveExceptionOrConflicts, cancellationToken);
        }

        private static async Task SendTo<T>(
            this IHubContext<RealtimeUpdatesHub> hub,
            T data,
            string method,
            CancellationToken cancellationToken)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            await hub.Clients.All.SendAsync(method,
                data,
                cancellationToken);
        }
    }
}