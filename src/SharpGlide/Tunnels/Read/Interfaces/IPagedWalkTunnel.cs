using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface IPagedWalkTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task>> WalkExpr { get; }
    }
    
    public interface IPagedWalkTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task>> WalkExpr { get; }
    }
}