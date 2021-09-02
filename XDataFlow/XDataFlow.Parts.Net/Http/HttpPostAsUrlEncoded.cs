using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Parts.Net.Http.Proxy;
using XDataFlow.Registry;

namespace XDataFlow.Parts.Net.Http
{
    public class HttpPostAsUrlEncoded : VectorPart<HttpPostAsUrlEncoded.Input, HttpPostAsUrlEncoded.Output>
    {
        private readonly IHttpClientProxy _httpClientProxy;

        public class Input
        {
            public string Url { get; set; }

            public Dictionary<string, string> FormData { get; set; }
        }
        
        public class Output
        {
            public string PageContent { get; set; }
        }

        public HttpPostAsUrlEncoded(IHttpClientProxy httpClientProxy, IDefaultRegistry defaultRegistry) : base(defaultRegistry) 
        {
            _httpClientProxy = httpClientProxy;
        }
 
        public override async Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            var pageData = await this._httpClientProxy.PostAsUrlEncodedAsync(data.Url, data.FormData, cancellationToken);

            this.Publish(new Output() {PageContent = pageData});
        }
    }
}