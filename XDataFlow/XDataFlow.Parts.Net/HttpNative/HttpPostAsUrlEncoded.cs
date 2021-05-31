using System.Collections.Generic;
using XDataFlow.Parts.Net.HttpNative.Proxy;

namespace XDataFlow.Parts.Net.HttpNative
{
    public class HttpPostAsUrlEncoded : FlowPart<HttpPostAsUrlEncoded.Input, HttpPostAsUrlEncoded.Output>
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

        public HttpPostAsUrlEncoded(IHttpClientProxy httpClientProxy)
        {
            _httpClientProxy = httpClientProxy;
        }

        protected override void ProcessMessage(Input data)
        {
            var pageData = this._httpClientProxy.PostAsUrlEncoded(data.Url, data.FormData).GetAwaiter().GetResult();

            this.Publish(new Output() {PageContent = pageData});
        }
    }
}