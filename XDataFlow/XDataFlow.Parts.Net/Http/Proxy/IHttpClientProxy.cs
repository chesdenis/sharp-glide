using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XDataFlow.Parts.Net.Http.Proxy
{
    public interface IHttpClientProxy
    {
        Task<string> GetAsync(string url, CancellationToken cancellationToken);

        Task<string> PostAsUrlEncodedAsync(string url, Dictionary<string, string> urlEncodedData, CancellationToken cancellationToken);
    }
}