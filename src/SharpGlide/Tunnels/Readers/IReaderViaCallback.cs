using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpGlide.Tunnels.Readers
{
    public interface IReaderViaCallback<T> : IReader<T>
    {
        Expression<Action<Action<T>>> ReadViaCallbackExpr { get; }

        Expression<Action<Action<IEnumerable<T>>>> ReadRangeViaCallbackExpr { get; }
    }
}