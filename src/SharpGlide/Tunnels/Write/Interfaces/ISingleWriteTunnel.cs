using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Write.Interfaces
{
    public interface ISingleWriteTunnel<T> : ITunnel
    {
        Expression<Func<T, IRoute, CancellationToken, Task>> WriteSingleExpr { get; }
        Expression<Func<T, IRoute, CancellationToken, Task<T>>> WriteAndReturnExpr { get; }
    }
    
    public interface ISingleWriteTunnel<T, TArg> : ITunnel
    {
        Expression<Func<TArg, T, IRoute, CancellationToken, Task>> WriteSingleExpr { get; }
        Expression<Func<TArg, T, IRoute, CancellationToken, Task<T>>> WriteAndReturnSingleExpr { get; }
    }
}