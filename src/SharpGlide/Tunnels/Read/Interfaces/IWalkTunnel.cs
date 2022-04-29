using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface IWalkTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, Action<T>, Task>> WalkExpr { get; }
        Expression<Func<CancellationToken, Func<CancellationToken, T, Task>, Task>> WalkAsyncExpr { get; }
        
        Expression<Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task>> WalkPagedExpr { get; }
        Expression<Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task>> WalkPagedAsyncExpr { get; }
    }
}