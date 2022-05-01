using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.SharedModel;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Extensions;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Abstractions;

namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk
{
    public class DiskSingleFileUploadTunnel : SingleWriteTunnel<IDiskFileUploadTunnel.IFileInformation,
        AuthorizeTokens>
    {
        private readonly HttpClient _httpClient;

        public DiskSingleFileUploadTunnel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected override async Task WriteImpl(
            AuthorizeTokens arg,
            IDiskFileUploadTunnel.IFileInformation data,
            IRoute route,
            CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.PopulateUploadUri(
                _httpClient, arg, 
                new List<IDiskFileUploadTunnel.IFileInformation>() { data },
                cancellationToken);
        }

        protected override async Task<IDiskFileUploadTunnel.IFileInformation> WriteAndReturnImpl(
            AuthorizeTokens arg,
            IDiskFileUploadTunnel.IFileInformation data,
            IRoute route,
            CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.PopulateUploadUri(
                _httpClient,
                arg, 
                new List<IDiskFileUploadTunnel.IFileInformation>() { data },
                cancellationToken);

            return data;
        }

        
    }
}