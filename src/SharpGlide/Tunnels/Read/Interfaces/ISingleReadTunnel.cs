using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Read.Interfaces
{
    public interface ISingleReadTunnel<T, TArg> : ITunnel
    {
        Expression<Func<CancellationToken, TArg, Task<T>>> ReadSingleExpr { get; }
    }

    public interface ISingleReadTunnel<T> : ITunnel
    {
        Expression<Func<CancellationToken, Task<T>>> ReadSingleExpr { get; }
    }
}