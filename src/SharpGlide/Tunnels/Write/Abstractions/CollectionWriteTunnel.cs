using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Interfaces;

namespace SharpGlide.Tunnels.Write.Abstractions
{
    public abstract class CollectionWriteTunnel<T> : ICollectionWriteTunnel<T>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task>> WriteCollectionExpr
            => (data, route, cancellationToken) => WriteCollectionImpl(data, route, cancellationToken);

        protected abstract Task WriteCollectionImpl(IEnumerable<T> data, IRoute route, CancellationToken cancellationToken);

        public Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturnCollectionExpr
            => (data, route, cancellationToken) => WriteAndReturnCollectionImpl(data, route, cancellationToken);

        protected abstract Task<IEnumerable<T>> WriteAndReturnCollectionImpl(IEnumerable<T> data, IRoute route,
            CancellationToken cancellationToken);
    }
    
    public abstract class CollectionWriteTunnel<T, TArg> : ICollectionWriteTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task>> WriteCollectionExpr
            => (arg, data, route, cancellationToken) => WriteCollectionImpl(arg, data, route, cancellationToken);

        protected abstract Task WriteCollectionImpl(TArg arg, IEnumerable<T> data, IRoute route, CancellationToken cancellationToken);

        public Expression<Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturnCollectionExpr
            => (arg, data, route, cancellationToken) => WriteAndReturnCollectionImpl(arg, data, route, cancellationToken);

        protected abstract Task<IEnumerable<T>> WriteAndReturnCollectionImpl(TArg arg, IEnumerable<T> data, IRoute route,
            CancellationToken cancellationToken);
    }
}