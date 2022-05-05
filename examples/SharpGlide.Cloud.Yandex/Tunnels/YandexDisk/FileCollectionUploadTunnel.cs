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
    public class FileCollectionUploadTunnel :
        CollectionWriteTunnel<ICloudFileInformation,
            IAuthorizeTokens>
    {
        private readonly HttpClient _httpClient;

        public FileCollectionUploadTunnel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected override async Task WriteCollectionImpl(IAuthorizeTokens arg, IEnumerable<ICloudFileInformation> data,
            IRoute route, CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.UploadFiles(
                _httpClient, arg,
                data,
                cancellationToken);
        }

        protected override async Task<IEnumerable<ICloudFileInformation>> WriteAndReturnCollectionImpl(
            IAuthorizeTokens arg, IEnumerable<ICloudFileInformation> data, IRoute route,
            CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.UploadFiles(
                _httpClient, arg,
                data,
                cancellationToken);

            return data;
        }
    }
}