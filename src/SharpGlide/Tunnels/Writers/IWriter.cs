using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        Expression<Action<Task<T>, IRoute>> WriteSingleExpr { get; }
        Expression<Action<Task<IEnumerable<T>>, IRoute>> WriteRangeExpr { get; }
    }
}