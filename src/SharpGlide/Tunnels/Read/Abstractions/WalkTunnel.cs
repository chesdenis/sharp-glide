using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class WalkTunnel<T> :
        ISingleWalkTunnel<T>,
        ISingleAsyncWalkTunnel<T>,
        IPagedWalkTunnel<T>,
        IPagedAsyncWalkTunnel<T>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<CancellationToken, Action<T>, Task>> WalkSingleExpr
            => (cancellationToken, callback) => SingleWalkImpl(cancellationToken, callback);

        public Expression<Func<CancellationToken, Func<CancellationToken, T, Task>, Task>> WalkSingleAsyncExpr
            => (cancellationToken, callback) => SingleAsyncWalkImpl(cancellationToken, callback);

        public Expression<Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task>> WalkPagedExpr
            => (cancellationToken, pageInfo, callback) => PagedWalkImpl(cancellationToken, pageInfo, callback);

        public Expression<Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task>>
            WalkPagedAsyncExpr
            => (cancellationToken, pageInfo, callback) => PagedAsyncWalkImpl(cancellationToken, pageInfo, callback);

        protected abstract Task SingleWalkImpl(CancellationToken cancellationToken, Action<T> callback);

        protected abstract Task SingleAsyncWalkImpl(CancellationToken cancellationToken,
            Func<CancellationToken, T, Task> callback);

        protected abstract Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Action<IEnumerable<T>> callback);

        protected abstract Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Func<CancellationToken, IEnumerable<T>, Task> callback);
    }

    public abstract class WalkTunnel<T, TArg> :
        ISingleWalkTunnel<T, TArg>,
        ISingleAsyncWalkTunnel<T, TArg>,
        IPagedWalkTunnel<T, TArg>,
        IPagedAsyncWalkTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<CancellationToken, TArg, Action<T>, Task>> WalkSingleExpr
            => (cancellationToken, request, callback) => SingleWalkImpl(cancellationToken, request, callback);

        public Expression<Func<CancellationToken, TArg, Func<CancellationToken, T, Task>, Task>> WalkSingleAsyncExpr
            => (cancellationToken, request, callback) => SingleAsyncWalkImpl(cancellationToken, request, callback);

        public Expression<Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task>> WalkPagedExpr
            => (cancellationToken, pageInfo, request, callback) =>
                PagedWalkImpl(cancellationToken, pageInfo, request, callback);

        public Expression<Func<CancellationToken, PageInfo, TArg, Func<CancellationToken, IEnumerable<T>, Task>, Task>>
            WalkPagedAsyncExpr
            => (cancellationToken, pageInfo, request, callback) =>
                PagedAsyncWalkImpl(cancellationToken, pageInfo, request, callback);


        protected abstract Task SingleWalkImpl(CancellationToken cancellationToken,
            TArg request, Action<T> callback);

        protected abstract Task SingleAsyncWalkImpl(CancellationToken cancellationToken,
            TArg request, Func<CancellationToken, T, Task> callback);


        protected abstract Task PagedWalkImpl(CancellationToken cancellationToken,
            PageInfo pageInfo, TArg request, Action<IEnumerable<T>> callback);


        protected abstract Task PagedAsyncWalkImpl(CancellationToken cancellationToken,
            PageInfo pageInfo, TArg request, Func<CancellationToken, IEnumerable<T>, Task> callback);
    }
}