using System.Collections.Generic;
using System.Linq;
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
    public class SingleFileUploadTunnel : SingleWriteTunnel<ICloudFileInformation,
        IAuthorizeTokens>
    {
        private readonly HttpClient _httpClient;

        public SingleFileUploadTunnel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected override async Task WriteSingleImpl(
            IAuthorizeTokens arg,
            ICloudFileInformation data,
            IRoute route,
            CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.CalculateFileUploadUri(
                _httpClient, arg, 
                new [] { data },
                cancellationToken);
        }

        protected override async Task<ICloudFileInformation> WriteAndReturnSingleImpl(
            IAuthorizeTokens arg,
            ICloudFileInformation data,
            IRoute route,
            CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.CalculateFileUploadUri(
                _httpClient,
                arg, 
                new [] { data },
                cancellationToken);

            return data;
        }

        
    }
}