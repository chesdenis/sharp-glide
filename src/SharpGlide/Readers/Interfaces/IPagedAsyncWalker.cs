using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers
{
    public interface IPagedAsyncWalker<out T>
    {
        Task WalkAsync(CancellationToken cancellationToken, PageInfo pageInfo,
            Func<CancellationToken, IEnumerable<T>, Task> callbackAsync);
    }
    
    public interface IPagedAsyncWalker<out T, in TArg>
    {
        Task WalkAsync(CancellationToken cancellationToken, PageInfo pageInfo, TArg request,
            Func<CancellationToken, IEnumerable<T>, Task> callbackAsync);
    }
}