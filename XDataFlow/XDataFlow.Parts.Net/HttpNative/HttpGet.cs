using XDataFlow.Parts.Net.HttpNative.Proxy;

namespace XDataFlow.Parts.Net.HttpNative
{
    public class HttpGet : FlowPart<HttpGet.Input, HttpGet.Output>
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
 
        protected override void ProcessMessage(Input data)
        {
            var pageData = this._httpClientProxy.Get(data.Url).GetAwaiter().GetResult();

            this.Publish(new Output() {PageContent = pageData});
        }
    }
}