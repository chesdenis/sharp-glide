using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Interfaces;

namespace SharpGlide.Tunnels.Write.Abstractions
{
    public abstract class WriteTunnel<T> : IWriteTunnel<T>
    {
        public bool CanExecute { get; set; }

        public virtual Expression<Func<T, IRoute, CancellationToken, Task>> WriteSingleExpr
            => (data, route, cancellationToken) => WriteSingleImpl(data, route, cancellationToken);
        protected abstract Task WriteSingleImpl(T data, IRoute route, CancellationToken cancellationToken);

        public virtual Expression<Func<T, IRoute, CancellationToken, Task<T>>> WriteAndReturnSingleExpr =>
            (data, route, cancellationToken) => WriteAndReturnSingleImpl(data, route, cancellationToken);

        protected abstract Task<T> WriteAndReturnSingleImpl(T data, IRoute route, CancellationToken cancellationToken);

        public Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task>> WriteRangeExpr
            => (dataRange, route, cancellationToken) => WriteRangeImpl(dataRange, route, cancellationToken);

        protected abstract Task WriteRangeImpl(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);

        public Expression<Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>>> WriteAndReturnRangeExpr
            => (dataRange, route, cancellationToken) => WriteAndReturnRangeImpl(dataRange, route, cancellationToken);

        protected abstract Task<IEnumerable<T>> WriteAndReturnRangeImpl(
            IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
    }
}