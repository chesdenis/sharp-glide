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

        public Expression<Func<CancellationToken, TRequest, Action<T>, Task>> WalkExpr =>
            (cancellationToken, request, callback) => WalkExprImpl(cancellationToken, request, callback);

        protected abstract Task WalkExprImpl(CancellationToken cancellationToken, TRequest request, Action<T> callback);

        public Expression<Func<CancellationToken, TRequest, Action<IEnumerable<T>>, Task>> WalkRangeExpr =>
            (cancellationToken, request, callback) => WalkRangeExprImpl(cancellationToken, request, callback);

        protected abstract Task WalkRangeExprImpl(CancellationToken cancellationToken, TRequest request, Action<IEnumerable<T>> callback);
        
        public Expression<Func<CancellationToken, PageInfo, TRequest, Action<IEnumerable<T>>, Task>> WalkPagedExpr
            => (cancellationToken, pageInfo, request, callback) => WalkPagedImpl(cancellationToken, pageInfo, request, callback);

        protected abstract Task WalkPagedImpl(CancellationToken cancellationToken, 
            PageInfo pageInfo, TRequest request, Action<IEnumerable<T>> callback);
    }
}