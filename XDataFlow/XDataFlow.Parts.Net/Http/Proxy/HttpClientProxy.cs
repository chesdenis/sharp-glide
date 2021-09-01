using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XDataFlow.Parts.Net.Http.Proxy
{
    public class HttpClientProxy : IHttpClientProxy
    {
        private readonly HttpClient _httpClient;

        public HttpClientProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAsync(string url, CancellationToken cancellationToken)
        {
            var httpResponseMessage = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url), cancellationToken);

            return await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        }

        public async Task<string> PostAsUrlEncodedAsync(string url, Dictionary<string, string> urlEncodedData, CancellationToken cancellationToken)
        {
            var httpResponseMessage = await _httpClient.PostAsync(url, new FormUrlEncodedContent(urlEncodedData), cancellationToken);
            
            return await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}