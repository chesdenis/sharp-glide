using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace XDataFlow.Parts.Net.HttpNative.Proxy
{
    public class HttpClientProxy : IHttpClientProxy
    {
        private readonly HttpClient _httpClient;

        public HttpClientProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Get(string url)
        {
            var httpResponseMessage = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
        
        public async Task<string> PostAsUrlEncoded(string url, Dictionary<string, string> urlEncodedData)
        {
            var httpResponseMessage = _httpClient.PostAsync(url, new FormUrlEncodedContent(urlEncodedData));
            
            return await httpResponseMessage.Result.Content.ReadAsStringAsync();
        }
    }
}