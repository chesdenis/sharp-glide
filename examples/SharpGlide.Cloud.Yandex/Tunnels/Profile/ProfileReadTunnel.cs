using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.Authorization.Extensions;
using SharpGlide.Cloud.Yandex.Tunnels.Profile.Model;
using SharpGlide.Extensions;
using SharpGlide.Tunnels.Read.Abstractions;

namespace SharpGlide.Cloud.Yandex.Tunnels.Profile
{
    public class ProfileReadTunnel : SingleReadTunnel<ProfileResponse, IAuthorizeTokens>, IProfileReadTunnel
    {
        private const string UserInformationEndpoint = "https://login.yandex.ru/info";

        private readonly HttpClient _httpClient;

        public ProfileReadTunnel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected override async Task<ProfileResponse> SingleReadImpl(CancellationToken cancellationToken, IAuthorizeTokens arg)
        {
            _httpClient.IncludeAccessToken(arg);

            var response = await _httpClient.GetAsync(UserInformationEndpoint.BindUriArgs("format", "json"),
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody =
                    await response.Content.ReadFromJsonAsync<ProfileResponse>(
                        cancellationToken: cancellationToken);

                if (responseBody == null)
                {
                    throw new ArgumentNullException(nameof(responseBody),
                        $"Unable to deserialize response from api when requesting user profile information");
                }

                return responseBody;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(response),
                    $"Status code is not success when requesting user profile information");
            }
        }
    }
}