using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface IReadWithArgTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, TArg, Task<T>>> ReadExpr { get; }

        Expression<Func<CancellationToken, TArg, Task<IEnumerable<T>>>> ReadAllExpr { get; }

        Expression<Func<CancellationToken, PageInfo, TArg, Task<IEnumerable<T>>>> ReadPagedExpr
        {
            get;
        }

        Expression<Func<CancellationToken, TArg, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>>
            ReadSpecificExpr { get; }
    }
}