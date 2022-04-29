using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class WalkTunnel<T> : IWalkTunnel<T>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<CancellationToken, Action<T>, Task>> WalkExpr =>
            (cancellationToken, callback) => WalkImpl(cancellationToken, callback);


        protected abstract Task WalkImpl(CancellationToken cancellationToken, Action<T> callback);

        public Expression<Func<CancellationToken, Func<CancellationToken, T, Task>, Task>> WalkAsyncExpr =>
            (cancellationToken, callback) => WalkAsyncImpl(cancellationToken, callback);

        protected abstract Task WalkAsyncImpl(CancellationToken cancellationToken,
            Func<CancellationToken, T, Task> callback);


        public Expression<Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task>> WalkPagedExpr =>
            (cancellationToken, pageInfo, callback) => WalkPagedImpl(cancellationToken, pageInfo, callback);

        protected abstract Task WalkPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Action<IEnumerable<T>> callback);

        public Expression<Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task>>
            WalkPagedAsyncExpr
            => (cancellationToken, pageInfo, callback) => WalkPagedAsyncImpl(cancellationToken, pageInfo, callback);

        protected abstract Task WalkPagedAsyncImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Func<CancellationToken, IEnumerable<T>, Task> callback);
    }
}