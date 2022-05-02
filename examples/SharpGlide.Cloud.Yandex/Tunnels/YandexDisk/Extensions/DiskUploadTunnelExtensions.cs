using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.Authorization.Extensions;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Extensions;

namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Extensions
{
    public static class DiskUploadTunnelExtensions
    {
        private const string GetUploadEndpointUri = "https://cloud-api.yandex.net/v1/disk/resources/upload";
        
        public static async Task PopulateUploadUri(
            HttpClient httpClient,
            IAuthorizeTokens arg,
            IEnumerable<IDiskFileUploadTunnel.IFileInformation> dataCollection,
            CancellationToken cancellationToken)
        {
            httpClient.IncludeAccessToken(arg);

            foreach (var data in dataCollection)
            {
                var uploadUrl = HttpUtility.UrlEncode(data.FullName);

                var response = await httpClient.GetAsync(
                    GetUploadEndpointUri.BindUriArgs("path", uploadUrl), cancellationToken);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseBody =
                        await response.Content.ReadFromJsonAsync<UploadUriResponse>(
                            cancellationToken: cancellationToken);

                    if (responseBody == null)
                    {
                        throw new ArgumentNullException(nameof(responseBody),
                            $"Unable to deserialize response from api when requesting upload uri for {data.FullName}");
                    }

                    data.UploadUri = responseBody.href;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(response),
                        $"Status code is not success when requesting file upload for {data.FullName}");
                }
            }
        }
    }
}