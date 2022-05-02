using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Model;
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

        protected override async Task WriteSingleImpl(
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

        protected override async Task<IDiskFileUploadTunnel.IFileInformation> WriteAndReturnSingleImpl(
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