using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface IPagedReadTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, PageInfo, TArg, Task<IEnumerable<T>>>> ReadPagedExpr { get; }
    }

    public interface IPagedReadTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, PageInfo, Task<IEnumerable<T>>>> ReadPagedExpr { get; }
    }
}