using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers.Interfaces
{
    public interface IPagedWalker<out T>
    {
        Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, Action<IEnumerable<T>> callback);
    }
    
    public interface IPagedWalker<out T, in TArg>
    {
        Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, TArg request, Action<IEnumerable<T>> callback);
    }
}