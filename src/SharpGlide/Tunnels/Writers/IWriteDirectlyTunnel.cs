using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Routing;

namespace SharpGlide.Tunnels.Writers
{
    public interface IWriteDirectlyTunnel<T> : IWriteTunnel<T>
    {
        Expression<Action<T, IRoute>> WriteExpr { get; }
        
        Expression<Action<IEnumerable<T>, IRoute>> WriteRangeExpr { get; }
    }
}