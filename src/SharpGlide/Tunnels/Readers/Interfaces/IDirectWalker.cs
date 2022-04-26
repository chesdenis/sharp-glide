using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tunnels.Readers.Interfaces
{
    public interface IDirectWalker<T> : IReader<T>
    {
        Expression<Func<CancellationToken, Task<Action<T>>>> WalkExpr { get; }
        
        Expression<Func<CancellationToken, Task<Action<IEnumerable<T>>>>> WalkPagedExpr { get; }
    }
}