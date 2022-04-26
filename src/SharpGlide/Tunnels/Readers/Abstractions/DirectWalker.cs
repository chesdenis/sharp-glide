using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Interfaces;

namespace SharpGlide.Tunnels.Readers.Abstractions
{
    public abstract class DirectWalker<T> : IDirectWalker<T>
    {
        public bool CanExecute { get; set; }


        public Expression<Func<CancellationToken, Task<Action<T>>>> WalkExpr => 
            (cancellationToken) => WalkImpl(cancellationToken);
        protected abstract Task<Action<T>> WalkImpl(CancellationToken cancellationToken);

        public Expression<Func<CancellationToken, Task<Action<IEnumerable<T>>>>> WalkPagedExpr =>
            (cancellationToken) => WalkPagedImpl(cancellationToken);

        protected abstract Task<Action<IEnumerable<T>>> WalkPagedImpl(CancellationToken cancellationToken);
    }
}