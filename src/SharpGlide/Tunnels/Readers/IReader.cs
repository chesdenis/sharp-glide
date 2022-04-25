using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Readers
{
    public interface IReader<T> : ITunnel
    {
       
    }

    public interface IOnDemandReaderV2<T> : IReader<T>
    {
        Expression<Func<CancellationToken, Task<T>>> ReadSingleExpr { get; }
        Expression<Func<CancellationToken, Task<IEnumerable<T>>>> ReadAllExpr { get; }
        Expression<Func<CancellationToken,  Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>> ReadRangeExpr { get; }
    }
}