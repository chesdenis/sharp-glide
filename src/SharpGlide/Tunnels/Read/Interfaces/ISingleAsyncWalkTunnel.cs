using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface ISingleAsyncWalkTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, Func<CancellationToken, T, Task>, Task>> WalkExpr { get; }
    }

    public interface ISingleAsyncWalkTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, TArg, Func<CancellationToken, T, Task>, Task>> WalkExpr { get; }
    }
}