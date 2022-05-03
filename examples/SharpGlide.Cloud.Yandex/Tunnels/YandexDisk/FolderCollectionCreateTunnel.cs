using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Extensions;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Abstractions;

namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk
{
    public class FolderCollectionCreateTunnel : CollectionWriteTunnel<ICloudFolderInformation, IAuthorizeTokens>
    {
        private readonly HttpClient _httpClient;

        public FolderCollectionCreateTunnel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected override async Task WriteCollectionImpl(IAuthorizeTokens arg,
            IEnumerable<ICloudFolderInformation> data, IRoute route, CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.CreateFolder(_httpClient, arg, data, cancellationToken);
        }

        protected override async Task<IEnumerable<ICloudFolderInformation>> WriteAndReturnCollectionImpl(
            IAuthorizeTokens arg, IEnumerable<ICloudFolderInformation> data, IRoute route,
            CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.CreateFolder(_httpClient, arg, data, cancellationToken);

            return data;
        }
    }
}