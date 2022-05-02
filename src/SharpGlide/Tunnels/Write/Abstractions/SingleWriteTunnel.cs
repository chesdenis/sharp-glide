using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Interfaces;

namespace SharpGlide.Tunnels.Write.Abstractions
{
    public abstract class SingleWriteTunnel<T, TArg> : ISingleWriteTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<TArg, T, IRoute, CancellationToken, Task>> WriteSingleExpr =>
            (arg, data, route, cancellationToken) => WriteSingleImpl(arg, data, route, cancellationToken);

        protected abstract Task WriteSingleImpl(TArg arg, T data, IRoute route, CancellationToken cancellationToken);

        public Expression<Func<TArg, T, IRoute, CancellationToken, Task<T>>> WriteAndReturnSingleExpr
            => (arg, data, route, cancellationToken) => WriteAndReturnSingleImpl(arg, data, route, cancellationToken);

        protected abstract Task<T> WriteAndReturnSingleImpl(TArg arg, T data, IRoute route,
            CancellationToken cancellationToken);
    }

    public abstract class SingleWriteTunnel<T> : ISingleWriteTunnel<T>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<T, IRoute, CancellationToken, Task>> WriteSingleExpr =>
            (data, route, cancellationToken) => WriteImpl(data, route, cancellationToken);

        protected abstract Task WriteImpl(T data, IRoute route, CancellationToken cancellationToken);

        public Expression<Func<T, IRoute, CancellationToken, Task<T>>> WriteAndReturnExpr
            => (data, route, cancellationToken) => WriteAndReturnImpl(data, route, cancellationToken);

        protected abstract Task<T> WriteAndReturnImpl(T data, IRoute route, CancellationToken cancellationToken);
    }
}