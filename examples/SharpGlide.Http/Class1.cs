using System;
using SharpGlide.Tunnels.Read.Abstractions;

namespace SharpGlide.Http
{
    public interface IHttpRequestReadTunnelArgs
    {
        string Method { get; set; }
        string Type { get; set; }
        string Parameters { get; set; }
    }
    // public class HttpRequestReadTunnel<T> : ReadTunnel<T, IHttpRequestReadTunnelArgs>
    // {
    //     public HttpRequestReadTunnel()
    //     {
    //         
    //     }
    // }
}