using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Parts.Net.Http.Proxy;

namespace XDataFlow.Parts.Net.Http
{
    public class HttpGet : VectorPart<HttpGet.Input, HttpGet.Output>
    {
        private readonly IHttpClientProxy _httpClientProxy;

        public class Input
        {
            public string Url { get; set; }
        }
        
        public class Output
        {
            public string PageContent { get; set; }
        }

        public HttpGet(IHttpClientProxy httpClientProxy)
        {
            _httpClientProxy = httpClientProxy;
        }
    
        public override async Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            var pageData = await this._httpClientProxy.GetAsync(data.Url, cancellationToken);

            this.Publish(new Output() {PageContent = pageData});
        }
    }
}