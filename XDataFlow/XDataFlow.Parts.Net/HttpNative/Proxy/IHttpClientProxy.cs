using System.Collections.Generic;
using System.Threading.Tasks;

namespace XDataFlow.Parts.Net.HttpNative.Proxy
{
    public interface IHttpClientProxy
    {
        Task<string> Get(string url);

        Task<string> PostAsUrlEncoded(string url, Dictionary<string, string> urlEncodedData);
    }
}