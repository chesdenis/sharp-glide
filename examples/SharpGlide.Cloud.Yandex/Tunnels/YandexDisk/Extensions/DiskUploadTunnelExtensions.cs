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
using SharpGlide.Cloud.Yandex.Extensions;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.Authorization.Extensions;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Extensions;

namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Extensions
{
    public static class DiskUploadTunnelExtensions
    {
        private const string GetFileUploadEndpointUri = "https://cloud-api.yandex.net/v1/disk/resources/upload";
        private const string CreateFolderEndpointUri = "https://cloud-api.yandex.net/v1/disk/resources";

        public static async Task CreateFolder(
            HttpClient httpClient,
            IAuthorizeTokens tokens,
            IEnumerable<ICloudFolderInformation> dataCollection,
            CancellationToken cancellationToken)
        {
            httpClient.IncludeAccessToken(tokens);

            foreach (var data in dataCollection)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var folderUrl = data.FullName.ToCloudPath();

                try
                {
                    var response = await httpClient.GetAsync(
                        CreateFolderEndpointUri.BindUriArgs("path", folderUrl), cancellationToken);

                    data.StatusCode = response.StatusCode.ToString();

                    var validatedResponse = await ValidateResponseAsync<
                        ICloudFolderInformation, string, FolderCreatedResponse>(
                        cancellationToken, response,
                        data, x => x.FullName);

                    data.CloudName = validatedResponse.href;
                }
                catch (Exception e)
                {
                    data.Reason = e.Message;
                }
            }
        }

        public static async Task UploadFileBytes(
            HttpClient httpClient,
            IAuthorizeTokens tokens,
            string uploadUri,
            ICloudFileBytesRange content,
            CancellationToken cancellationToken)
        {
            httpClient.IncludeAccessToken(tokens);

            try
            {
                var response = await httpClient.PutAsync(uploadUri, new ByteArrayContent(content.Bytes), cancellationToken);
                var validatedResponse = ValidateResponseAsync<ICloudFileBytesRange, string, UploadUriResponse>(
                    cancellationToken, response, content, arg => arg.CloudName);
            }
            catch (Exception e)
            {
                content.Reason = e.Message;
            }
        }

        public static async Task CalculateFileUploadUri(
            HttpClient httpClient,
            IAuthorizeTokens tokens,
            IEnumerable<ICloudFileInformation> dataCollection,
            CancellationToken cancellationToken)
        {
            httpClient.IncludeAccessToken(tokens);

            foreach (var data in dataCollection)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var uploadUrl = data.FullName.ToCloudPath();

                try
                {
                    var response = await httpClient.GetAsync(
                        GetFileUploadEndpointUri.BindUriArgs("path", uploadUrl), cancellationToken);

                    data.StatusCode = response.StatusCode.ToString();

                    var validateResponse =
                        await ValidateResponseAsync<ICloudFileInformation, string, UploadUriResponse>(
                            cancellationToken,
                            response,
                            data, x => x.FullName);

                    data.UploadUri = validateResponse.href;
                }
                catch (Exception e)
                {
                    data.Reason = e.Message;
                }
            }
        }

        private static async Task<TResponse> ValidateResponseAsync<T, THeader, TResponse>(
            CancellationToken cancellationToken, HttpResponseMessage response,
            T data, Func<T, THeader> dataHeader)
        {
            if (response.StatusCode == HttpStatusCode.OK
                ||
                response.StatusCode == HttpStatusCode.Created
                ||
                response.StatusCode == HttpStatusCode.Accepted)
            {
                var responseBody =
                    await response.Content.ReadAsStringAsync(cancellationToken);

                var responseTyped = JsonSerializer.Deserialize<TResponse>(responseBody);

                if (responseTyped == null)
                {
                    throw new ArgumentNullException(nameof(responseBody),
                        $"Unable to deserialize response for {dataHeader(data)}");
                }

                return responseTyped;
            }

            var responseText = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ArgumentException(nameof(response),
                $"Status code is not success for {dataHeader(data)}. Reason {responseText}");
        }
    }
}