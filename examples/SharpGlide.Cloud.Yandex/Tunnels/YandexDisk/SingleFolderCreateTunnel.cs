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
    public class SingleFolderCreateTunnel : SingleWriteTunnel<ICloudFolderInformation, IAuthorizeTokens>
    {
        private readonly HttpClient _httpClient;

        public SingleFolderCreateTunnel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        protected override async Task WriteSingleImpl(IAuthorizeTokens arg, ICloudFolderInformation data, IRoute route,
            CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.CreateFolder(_httpClient, arg,
                new[] { data }, cancellationToken);
        }

        protected override async Task<ICloudFolderInformation> WriteAndReturnSingleImpl(IAuthorizeTokens arg, ICloudFolderInformation data, IRoute route,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}