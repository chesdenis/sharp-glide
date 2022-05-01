using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface IPagedAsyncWalkTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, PageInfo, TArg, Func<CancellationToken, IEnumerable<T>, Task>, Task>> WalkExpr { get; }
    }

    public interface IPagedAsyncWalkTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task>> WalkExpr
        {
            get;
        }
    }
}