using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    // public class DirectReaderWithCustomExpression : IDirectReader<decimal>
    // {
    //     public bool CanExecute { get; set; } = true;
    //     public Expression<Func<decimal>> ReadExpr { get; }
    //     public Expression<Func<Task<decimal>>> ReadAsyncExpr { get; }
    //
    //     public DirectReaderWithCustomExpression(IDirectReader<decimal> innerLogic)
    //     {
    //         ReadExpr = Expression.Lambda<Func<decimal>>(GetLogic());
    //     }
    //
    //     private Expression GetLogic()
    //     {
    //         var blockExpr = Expression.Block(
    //             Expression.Call(
    //                 null,
    //                 typeof(Console).GetMethod("Write", new Type[] { typeof(String) }),
    //                 Expression.Constant("Hello ")
    //             ),
    //             Expression.Call(
    //                 null,
    //                 typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) }),
    //                 Expression.Constant("World!")
    //             ),
    //             Expression.Constant(10m)
    //         );
    //
    //         return blockExpr;
    //     }
    //
    //     public Expression<Func<IEnumerable<decimal>>> ReadRangeExpr { get; }
        
        //     // Add the following directive to the file:
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
//
// // Compile and run an expression tree.
//     int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);
//
//     Console.WriteLine(factorial);
    // }
}