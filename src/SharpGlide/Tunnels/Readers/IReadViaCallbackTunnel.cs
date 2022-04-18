using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpGlide.Tunnels.Readers
{
    public interface IReadViaCallbackTunnel<T> : IReadTunnel<T>
    {
        Expression<Action<Action<T>>> ConsumeWithCallbackExpr { get; }

        Expression<Action<Action<IEnumerable<T>>>> ConsumeRangeWithCallbackExpr { get; }
    }
}