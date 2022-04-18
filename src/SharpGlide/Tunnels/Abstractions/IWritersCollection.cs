using System.Collections.Generic;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Writers;

namespace SharpGlide.Tunnels.Abstractions
{
    public interface IWritersCollection<T> : IList<IWriteDirectlyTunnel<T>>
    {
    }
}