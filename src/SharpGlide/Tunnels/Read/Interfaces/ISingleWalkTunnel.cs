using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface ISingleWalkTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, Action<T>, Task>> WalkSingleExpr { get; }
    }
    
    public interface ISingleWalkTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, TArg, Action<T>, Task>> WalkSingleExpr { get; }
    }
}