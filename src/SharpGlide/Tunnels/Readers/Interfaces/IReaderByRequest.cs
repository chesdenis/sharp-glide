using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Interfaces
{
    public interface IReaderByRequest<T, TRequest> : IReader<T>
    {
        Expression<Func<CancellationToken, TRequest, Task<T>>> ReadExpr { get; }

        Expression<Func<CancellationToken, TRequest, Task<IEnumerable<T>>>> ReadAllExpr { get; }

        Expression<Func<CancellationToken, TRequest, Func<Task<IEnumerable<T>>, PageInfo>, PageInfo>> ReadPagedExpr
        {
            get;
        }

        Expression<Func<CancellationToken, TRequest, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>>
            ReadSpecificExpr { get; }
    }
}