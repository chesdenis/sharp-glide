using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Writers
{
    public interface IWriter<T> : ITunnel
    {
    }

    public interface IOnDemandWriterV2<T> : IWriter<T>
    {
        Expression<Func<T, IRoute, CancellationToken, Task>> WriteSingleExpr { get; }
        Expression<Func<T, IRoute, CancellationToken, Task<T>>> WriteAndReturnSingleExpr { get; }
        Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task>> WriteRangeExpr { get; }
        Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturnRangeExpr { get; }
    }
}