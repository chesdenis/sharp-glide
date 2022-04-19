using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpGlide.Tunnels.Readers
{
    public interface IReaderViaEvent<T> : IReader<T>
    {
        event EventHandler<decimal> OnRead;
        
        event EventHandler<IEnumerable<decimal>> OnReadRange;
        
        Expression<Action> ReadViaEventExpr { get; }

        Expression<Action> ReadRangeViaEventCallbackExpr { get; }
    }
}