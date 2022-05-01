using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Write.Interfaces
{
    public interface ICollectionWriteTunnel<T, TArg> : ITunnel
    {
        Expression<Func<TArg, IEnumerable<T> , IRoute, CancellationToken, Task>> Write { get; }
        Expression<Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturn { get; }
    }

    public interface ICollectionWriteTunnel<T> : ITunnel
    {
        Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task>> Write { get; }
        Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturn { get; }
    }
}