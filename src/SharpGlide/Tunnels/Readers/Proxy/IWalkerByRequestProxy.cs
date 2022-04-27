using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public interface IWalkerByRequestProxy<T, in TRequest>
    {
        Task WalkAsync(CancellationToken cancellationToken, TRequest request, Action<T> callback);
        Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, TRequest request, Action<IEnumerable<T>> callback);
    }
}