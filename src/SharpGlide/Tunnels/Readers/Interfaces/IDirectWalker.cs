using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Interfaces
{
    public interface IDirectWalker<T> : IReader<T>
    {
        Expression<Func<CancellationToken, Action<T>, Task>> WalkExpr { get; }
        
        Expression<Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task>> WalkPagedExpr { get; }
    }
}