using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SharpGlide.Tunnels.Readers
{
    public interface IDirectReaderAsync<T> : IReader<T>
    {
        Expression<Func<Task<T>>> ReadAsyncExpr { get; }

        Expression<Func<Task<IEnumerable<T>>>> ReadRangeAsyncExpr { get; }
    }
}