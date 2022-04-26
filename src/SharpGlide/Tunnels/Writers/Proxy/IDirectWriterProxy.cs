using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;

namespace SharpGlide.Tunnels.Writers.Proxy
{
    public interface IDirectWriterProxy<T>
    {
        Task WriteSingle(T data, IRoute route, CancellationToken cancellationToken);
        Task<T> WriteAndReturnSingle(T data, IRoute route, CancellationToken cancellationToken);
        Task WriteRange(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
        Task<IEnumerable<T>> WriteAndReturnRange(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
    }
}