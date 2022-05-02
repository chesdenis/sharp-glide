using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Extensions;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Abstractions;

namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk
{
    public class DiskCollectionFileUploadTunnel : 
        CollectionWriteTunnel<IDiskFileUploadTunnel.IFileInformation,
        IAuthorizeTokens>
    {
        private readonly HttpClient _httpClient;

        public DiskCollectionFileUploadTunnel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        protected override async Task WriteCollectionImpl(IAuthorizeTokens arg, IEnumerable<IDiskFileUploadTunnel.IFileInformation> data, IRoute route, CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.PopulateUploadUri(
                _httpClient, arg, 
                data,
                cancellationToken);
        }

        protected override async Task<IEnumerable<IDiskFileUploadTunnel.IFileInformation>> WriteAndReturnCollectionImpl(IAuthorizeTokens arg, IEnumerable<IDiskFileUploadTunnel.IFileInformation> data, IRoute route, CancellationToken cancellationToken)
        {
            await DiskUploadTunnelExtensions.PopulateUploadUri(
                _httpClient, arg, 
                data,
                cancellationToken);

            return data;
        }
    }
}