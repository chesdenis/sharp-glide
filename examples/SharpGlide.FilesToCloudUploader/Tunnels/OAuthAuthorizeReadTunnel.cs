using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Extensions;
using SharpGlide.Readers;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.FilesToCloudUploader.Tunnels
{
    public class OAuthAuthorizeReadTunnel : ReadTunnel<OAuthAuthorizeReadTunnel.OAuthResponse,
        OAuthAuthorizeReadTunnel.OAuthRequest>
    {
        private static readonly HttpClient WebClient = new();
        private const string AuthorizeEndpoint = "https://oauth.yandex.ru/authorize";

        public class OAuthRequest
        {
            public string ClientId { get; set; }
            public string SecretId { get; set; }
            public string RefreshToken { get; set; }
        }

        public class OAuthResponse
        {
            public string AuthorizationCode { get; set; }
        }

        // protected override async Task<OAuthResponse> ReadSingleImpl(CancellationToken cancellationToken,
        //     OAuthRequest request)
        // {
        //     var uri = "https://cloud-api.yandex.net/v1/disk/resources/upload".BindUriArgs("path", "test/test2"); 
        //     var webRequest = WebRequest.Create(uri);
        //     webRequest.Method = "GET";
        //     webRequest.ContentType = "application/json";
        //     webRequest.Headers.Add(HttpRequestHeader.Accept, "*/*");
        //   
        //     var authorizationValue =
        //         $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{request.ClientId}:{request.SecretId}"))}";
        //     webRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationValue);
        //
        //     var webResponse = await webRequest.GetResponseAsync();
        //     
        //     //
        //     // WebClient.DefaultRequestHeaders.Authorization =
        //     //     new AuthenticationHeaderValue());
        //     //
        //     // var response = await WebClient.GetAsync(
        //     //    ,
        //     //     cancellationToken);
        //     //
        //     // if (response.StatusCode != HttpStatusCode.OK)
        //     // {
        //     //     throw new ArgumentException(nameof(response), response.ReasonPhrase);
        //     // }
        //     //
        //     // var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        //
        //     return await Task.FromResult(new OAuthResponse());
        // }


        protected override async Task<OAuthResponse> SingleReadImpl(CancellationToken cancellationToken, OAuthRequest arg)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<OAuthResponse>> CollectionReadImpl(CancellationToken cancellationToken, OAuthRequest arg)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<OAuthResponse>> PagedReadImpl(CancellationToken cancellationToken, PageInfo pageInfo, OAuthRequest arg)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<OAuthResponse>> FilteredReadImpl(CancellationToken cancellationToken, OAuthRequest arg, Func<IEnumerable<OAuthResponse>, IEnumerable<OAuthResponse>> filter)
        {
            throw new NotImplementedException();
        }
    }
}