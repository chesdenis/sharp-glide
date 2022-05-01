using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface ICollectionReadTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, TArg, Task<IEnumerable<T>>>> ReadExpr { get; }
    }

    public interface ICollectionReadTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, Task<IEnumerable<T>>>> ReadExpr { get; }
    }
}