using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public interface IDirectWalkerProxy<T>
    {
        Task<Action<T>> WalkAsync(CancellationToken cancellationToken);
        Task<Action<IEnumerable<T>>> WalkPagedAsync(CancellationToken cancellationToken);
    }
}