using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers
{
    public interface IWalkerWithArg<T, in TArg>
    {
        Task WalkAsync(CancellationToken cancellationToken, TArg request, Action<T> callback);
        Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, TArg request, Action<IEnumerable<T>> callback);
    }
}