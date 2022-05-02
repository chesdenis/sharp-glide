using System.Net.Http;
using System.Net.Http.Headers;
using SharpGlide.Cloud.Yandex.Model;

namespace SharpGlide.Cloud.Yandex.Tunnels.Authorization.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void IncludeAccessToken(this HttpClient httpClient, AuthorizeTokens tokens)
        {
            httpClient.DefaultRequestHeaders.Authorization ??= new AuthenticationHeaderValue("OAuth", tokens.AccessToken);
        }
    }
}