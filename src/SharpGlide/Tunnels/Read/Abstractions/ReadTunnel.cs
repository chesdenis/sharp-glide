using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class ReadTunnel<T> : IReadTunnel<T>
    {
        public bool CanExecute { get; set; }
        public virtual Expression<Func<CancellationToken, Task<T>>> ReadExpr => 
            (cancellationToken) 
                => ReadSingleImpl(cancellationToken);
        protected abstract Task<T> ReadSingleImpl(CancellationToken cancellationToken);

        public virtual Expression<Func<CancellationToken, Task<IEnumerable<T>>>> ReadAllExpr =>
            (cancellationToken) 
                => ReadAllImpl(cancellationToken);
        protected abstract Task<IEnumerable<T>> ReadAllImpl(CancellationToken cancellationToken);

        public Expression<Func<CancellationToken, PageInfo, Task<IEnumerable<T>>>> ReadPagedExpr =>
            ((cancellationToken, pageInfo) => ReadPagedImpl(cancellationToken, pageInfo));

        protected abstract Task<IEnumerable<T>> ReadPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo);
        
        public virtual Expression<Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>>
            ReadSpecificExpr => (cancellationToken, filter) => ReadSpecificImpl(cancellationToken, filter);

        protected abstract Task<IEnumerable<T>> ReadSpecificImpl(
            CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
 
        //             // Add the following directive to the file:
// // using System.Linq.Expressions;
//
// // Creating a parameter expression.
//     ParameterExpression value = Expression.Parameter(typeof(int), "value");
//
// // Creating an expression to hold a local variable.
//     ParameterExpression result = Expression.Parameter(typeof(int), "result");
//
// // Creating a label to jump to from a loop.
//     LabelTarget label = Expression.Label(typeof(int));
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
}