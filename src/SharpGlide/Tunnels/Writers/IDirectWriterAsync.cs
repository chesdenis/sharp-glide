using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SharpGlide.Routing;

namespace SharpGlide.Tunnels.Writers
{
    public interface IDirectWriterAsync<T> : IWriter<T>
    {
        Expression<Task<Action<T, IRoute>>> WriteAsyncExpr { get; }
        
        Expression<Task<Action<IEnumerable<T>, IRoute>>> WriteRangeAsyncExpr { get; }
    }
}