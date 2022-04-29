using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers
{
    public interface IWalker<T>
    {
        Task WalkAsync(CancellationToken cancellationToken, Action<T> callback);
        Task WalkAsync(CancellationToken cancellationToken, Func<CancellationToken,T, Task> callbackAsync);
        Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, Action<IEnumerable<T>> callback);
        Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, Func<CancellationToken,IEnumerable<T>, Task> callbackAsync);
    }
}