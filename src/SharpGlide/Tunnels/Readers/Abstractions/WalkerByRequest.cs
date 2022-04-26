using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Interfaces;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Abstractions
{
    public abstract class WalkerByRequest<T, TRequest> : IWalkerByRequest<T, TRequest>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<CancellationToken, TRequest, Task<Action<T>>>> WalkExpr =>
            (cancellationToken, request) => WalkExprImpl(cancellationToken, request);

        protected abstract Task<Action<T>> WalkExprImpl(CancellationToken cancellationToken, TRequest request);

        public Expression<Func<CancellationToken, TRequest, Task<Action<IEnumerable<T>, PageInfo>>>> WalkPagedExpr
            => (cancellationToken, request) => WalkPagedImpl(cancellationToken, request);

        protected abstract Task<Action<IEnumerable<T>, PageInfo>> WalkPagedImpl(CancellationToken cancellationToken, TRequest request);
    }
}