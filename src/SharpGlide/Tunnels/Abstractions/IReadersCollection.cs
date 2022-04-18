using System.Collections.Generic;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Readers;

namespace SharpGlide.Tunnels.Abstractions
{
    public interface IReadersCollection<T> : IList<IReadTunnel<T>>
    {
    }
}