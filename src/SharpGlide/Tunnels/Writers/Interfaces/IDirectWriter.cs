using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;

namespace SharpGlide.Tunnels.Writers.Interfaces
{
    public interface IDirectWriter<T> : IWriter<T>
    {
        Expression<Func<T, IRoute, CancellationToken, Task>> WriteSingleExpr { get; }
        Expression<Func<T, IRoute, CancellationToken, Task<T>>> WriteAndReturnSingleExpr { get; }
        Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task>> WriteRangeExpr { get; }
        Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturnRangeExpr { get; }
    }
}