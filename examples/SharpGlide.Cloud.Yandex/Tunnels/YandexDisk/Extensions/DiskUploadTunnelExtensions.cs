using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            IEnumerable<ICloudFileInformation> dataCollection,
            CancellationToken cancellationToken)
        {
            httpClient.IncludeAccessToken(arg);

            foreach (var data in dataCollection)
            {
                var encodedPath = data.FullName
                    .Replace("C:\\", string.Empty).Replace("\\", "/").TrimStart('/');
                var uploadUrl = HttpUtility.UrlEncode('/' + encodedPath);

                var response = await httpClient.GetAsync(
                    GetUploadEndpointUri.BindUriArgs("path", uploadUrl), cancellationToken);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseBody =
                        await response.Content.ReadAsStringAsync(cancellationToken);

                    var responseTyped = JsonSerializer.Deserialize<UploadUriResponse>(responseBody);

                    if (responseTyped == null)
                    {
                        throw new ArgumentNullException(nameof(responseBody),
                            $"Unable to deserialize response from api when requesting upload uri for {data.FullName}");
                    }

                    data.UploadUri = responseTyped.href;
                }
                else
                {
                    var responseText = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new ArgumentOutOfRangeException(nameof(response),
                        $"Status code is not success when requesting file upload for {data.FullName}. Reason {responseText}");
                }
            }
        }
    }
}