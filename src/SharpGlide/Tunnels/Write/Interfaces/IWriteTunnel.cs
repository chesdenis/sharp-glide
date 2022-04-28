using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Write.Interfaces
{
    public interface IWriteTunnel<T> : ITunnel
    {
        Expression<Func<T, IRoute, CancellationToken, Task>> WriteSingleExpr { get; }
        Expression<Func<T, IRoute, CancellationToken, Task<T>>> WriteAndReturnSingleExpr { get; }
        Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task>> WriteRangeExpr { get; }
        Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturnRangeExpr { get; }
    }
}