using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class ReadTunnel<T> :
        ISingleReadTunnel<T>,
        ICollectionReadTunnel<T>,
        IPagedReadTunnel<T>,
        IFilteredReadTunnel<T>
    {
        Expression<Func<CancellationToken, Task<T>>> ISingleReadTunnel<T>.ReadExpr =>
            (cancellationToken)
                => SingleReadImpl(cancellationToken);

        Expression<Func<CancellationToken, Task<IEnumerable<T>>>> ICollectionReadTunnel<T>.ReadExpr =>
            (cancellationToken)
                => CollectionReadImpl(cancellationToken);

        Expression<Func<CancellationToken, PageInfo, Task<IEnumerable<T>>>> IPagedReadTunnel<T>.ReadExpr =>
            ((cancellationToken, pageInfo) => PagedReadImpl(cancellationToken, pageInfo));

        Expression<Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>>
            IFilteredReadTunnel<T>.ReadExpr =>
            (cancellationToken, filter) => FilteredReadImpl(cancellationToken, filter);

        protected abstract Task<T> SingleReadImpl(CancellationToken cancellationToken);
        protected abstract Task<IEnumerable<T>> CollectionReadImpl(CancellationToken cancellationToken);
        protected abstract Task<IEnumerable<T>> PagedReadImpl(CancellationToken cancellationToken, PageInfo pageInfo);

        protected abstract Task<IEnumerable<T>> FilteredReadImpl(CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter);

        public bool CanExecute { get; set; }
    }

    public abstract class ReadTunnel<T, TArg> :
        ISingleReadTunnel<T, TArg>,
        ICollectionReadTunnel<T, TArg>,
        IPagedReadTunnel<T, TArg>,
        IFilteredReadTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }

        Expression<Func<CancellationToken, TArg, Task<T>>> ISingleReadTunnel<T, TArg>.ReadExpr
            => (cancellationToken, arg)
                => SingleReadImpl(cancellationToken, arg);

        Expression<Func<CancellationToken, TArg, Task<IEnumerable<T>>>> ICollectionReadTunnel<T, TArg>.ReadExpr
            => (cancellationToken, arg) => CollectionReadImpl(cancellationToken, arg);

        Expression<Func<CancellationToken, PageInfo, TArg, Task<IEnumerable<T>>>> IPagedReadTunnel<T, TArg>.ReadExpr
            => (cancellationToken, pageInfo, arg) => PagedReadImpl(cancellationToken, pageInfo, arg);

        Expression<Func<CancellationToken, TArg, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>>
            IFilteredReadTunnel<T, TArg>.ReadExpr
            => (cancellationToken, arg, filter) => FilteredReadImpl(cancellationToken, arg, filter);

        protected abstract Task<T> SingleReadImpl(CancellationToken cancellationToken, TArg arg);
        protected abstract Task<IEnumerable<T>> CollectionReadImpl(CancellationToken cancellationToken, TArg arg);

        protected abstract Task<IEnumerable<T>> PagedReadImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            TArg arg);

        protected abstract Task<IEnumerable<T>> FilteredReadImpl(CancellationToken cancellationToken,
            TArg arg,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
    }
    
    

//
// // Creating a method body.
//     BlockExpression block = Expression.Block(
//         new[] { result },
//         Expression.Assign(result, Expression.Constant(1)),
//         Expression.Loop(
//             Expression.IfThenElse(
//                 Expression.GreaterThan(value, Expression.Constant(1)),
//                 Expression.MultiplyAssign(result,
//                     Expression.PostDecrementAssign(value)),
//                 Expression.Break(label, result)
//             ),
//             label
//         )
//     );
}