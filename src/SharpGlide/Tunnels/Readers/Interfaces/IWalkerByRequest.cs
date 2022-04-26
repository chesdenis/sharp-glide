using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Interfaces
{
    public interface IWalkerByRequest<T, TRequest> : IReader<T>
    {
        Expression<Func<CancellationToken, TRequest, Task<Action<T>>>> WalkExpr { get; }
        
        Expression<Func<CancellationToken, TRequest, Task<Action<IEnumerable<T>, PageInfo>>>> WalkPagedExpr { get; }
    }
}