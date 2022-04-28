using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class WalkWithArgTunnel<T, TArg> : IWalkWithArgTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<CancellationToken, TArg, Action<T>, Task>> WalkExpr =>
            (cancellationToken, request, callback) => WalkExprImpl(cancellationToken, request, callback);

        protected abstract Task WalkExprImpl(CancellationToken cancellationToken, TArg request, Action<T> callback);

        public Expression<Func<CancellationToken, TArg, Action<IEnumerable<T>>, Task>> WalkRangeExpr =>
            (cancellationToken, request, callback) => WalkRangeExprImpl(cancellationToken, request, callback);

        protected abstract Task WalkRangeExprImpl(CancellationToken cancellationToken, TArg request, Action<IEnumerable<T>> callback);
        
        public Expression<Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task>> WalkPagedExpr
            => (cancellationToken, pageInfo, request, callback) => WalkPagedImpl(cancellationToken, pageInfo, request, callback);

        protected abstract Task WalkPagedImpl(CancellationToken cancellationToken, 
            PageInfo pageInfo, TArg request, Action<IEnumerable<T>> callback);
    }
}