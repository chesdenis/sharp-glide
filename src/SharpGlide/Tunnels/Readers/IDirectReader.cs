using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpGlide.Tunnels.Readers
{
    public interface IDirectReader<T> : IReader<T>
    {
        Expression<Func<T>> ReadExpr { get; }
        
        Expression<Func<IEnumerable<T>>> ReadRangeExpr { get; }
    }
}