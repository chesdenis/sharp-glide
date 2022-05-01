using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Http
{
    // public interface IHttpRequestReadTunnelArgs
    // {
    //     string SessionId { get; set; }
    //     string Method { get; set; }
    //     string Type { get; set; }
    //     string Parameters { get; set; }
    // }
    // public class HttpRequestReadTunnel<T, IHttpRequestReadTunnelArgs> : ReadTunnel<T, IHttpRequestReadTunnelArgs>
    // {
    //     private readonly HttpClient _httpClient;
    //
    //     private readonly ConcurrentDictionary<string, List<IHttpRequestReadTunnelArgs>>
    //         _requestStore = new ConcurrentDictionary<string, List<IHttpRequestReadTunnelArgs>>();
    //
    //     public HttpRequestReadTunnel(HttpClient httpClient)
    //     {
    //         _httpClient = httpClient;
    //     }
    //
    //     protected override async Task<T> SingleReadImpl(CancellationToken cancellationToken, IHttpRequestReadTunnelArgs arg)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     protected override async Task<IEnumerable<T>> CollectionReadImpl(CancellationToken cancellationToken, IHttpRequestReadTunnelArgs arg)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     protected override async Task<IEnumerable<T>> PagedReadImpl(CancellationToken cancellationToken, PageInfo pageInfo, IHttpRequestReadTunnelArgs arg)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     protected override async Task<IEnumerable<T>> FilteredReadImpl(CancellationToken cancellationToken, IHttpRequestReadTunnelArgs arg, Func<IEnumerable<T>, IEnumerable<T>> filter)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}