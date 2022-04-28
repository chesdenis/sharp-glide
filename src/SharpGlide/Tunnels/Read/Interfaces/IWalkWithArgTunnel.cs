using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface IWalkWithArgTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, TArg, Action<T>, Task>> WalkExpr { get; }
        Expression<Func<CancellationToken, TArg, Action<IEnumerable<T>>, Task>> WalkRangeExpr { get; }
        
        Expression<Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task>> WalkPagedExpr { get; }
    }
}