using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public interface IDirectWalkerProxy<T>
    {
        Task WalkAsync(CancellationToken cancellationToken, Action<T> callback);
        Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, Action<IEnumerable<T>> callback);
    }
}