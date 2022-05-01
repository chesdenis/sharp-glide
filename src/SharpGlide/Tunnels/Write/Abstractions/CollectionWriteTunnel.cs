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

        public Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task>> Write
            => (data, route, cancellationToken) => WriteImpl(data, route, cancellationToken);

        protected abstract Task WriteImpl(IEnumerable<T> data, IRoute route, CancellationToken cancellationToken);

        public Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturn
            => (data, route, cancellationToken) => WriteAndReturnImpl(data, route, cancellationToken);

        protected abstract Task<IEnumerable<T>> WriteAndReturnImpl(IEnumerable<T> data, IRoute route,
            CancellationToken cancellationToken);
    }
    
    public abstract class CollectionWriteTunnel<T, TArg> : ICollectionWriteTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task>> Write
            => (arg, data, route, cancellationToken) => WriteImpl(arg, data, route, cancellationToken);

        protected abstract Task WriteImpl(TArg arg, IEnumerable<T> data, IRoute route, CancellationToken cancellationToken);

        public Expression<Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturn
            => (arg, data, route, cancellationToken) => WriteAndReturnImpl(arg, data, route, cancellationToken);

        protected abstract Task<IEnumerable<T>> WriteAndReturnImpl(TArg arg, IEnumerable<T> data, IRoute route,
            CancellationToken cancellationToken);
    }
}