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
        public Expression<Func<CancellationToken, Task<T>>> ReadSingleExpr =>
            (cancellationToken)
                => SingleReadImpl(cancellationToken);

        public Expression<Func<CancellationToken, Task<IEnumerable<T>>>> ReadCollectionExpr =>
            (cancellationToken)
                => CollectionReadImpl(cancellationToken);

        public Expression<Func<CancellationToken, PageInfo, Task<IEnumerable<T>>>> ReadPagedExpr =>
            ((cancellationToken, pageInfo) => PagedReadImpl(cancellationToken, pageInfo));

        public Expression<Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>> ReadFilteredExpr =>
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

        public Expression<Func<CancellationToken, TArg, Task<T>>> ReadSingleExpr
            => (cancellationToken, arg)
                => SingleReadImpl(cancellationToken, arg);

        public Expression<Func<CancellationToken, TArg, Task<IEnumerable<T>>>> ReadCollectionExpr
            => (cancellationToken, arg) => CollectionReadImpl(cancellationToken, arg);

        public Expression<Func<CancellationToken, PageInfo, TArg, Task<IEnumerable<T>>>> ReadPagedExpr
            => (cancellationToken, pageInfo, arg) => PagedReadImpl(cancellationToken, pageInfo, arg);

        public Expression<Func<CancellationToken, TArg, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>> ReadFilteredExpr
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