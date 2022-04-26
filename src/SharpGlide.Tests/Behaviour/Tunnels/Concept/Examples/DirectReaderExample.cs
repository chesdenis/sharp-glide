using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Exceptions;
using SharpGlide.Tunnels.Readers;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    // public class DirectReaderExample : IDirectReader<decimal>
    // {
    //     public readonly ConcurrentStack<decimal> Stack = new();
    //
    //     public Expression<Func<decimal>> ReadExpr => () => ReadLogic();
    //     public Expression<Func<Task<decimal>>> ReadAsyncExpr => () => ReadLogicAsync();
    //
    //     private decimal ReadLogic()
    //     {
    //         decimal data;
    //
    //         var success = Stack.TryPop(out data);
    //
    //         if (!success) throw new NoDataException();
    //
    //         return data;
    //     }
    //     
    //     private Task<decimal> ReadLogicAsync()
    //     {
    //         decimal data;
    //
    //         var success = Stack.TryPop(out data);
    //
    //         if (!success) throw new NoDataException();
    //
    //         return Task.FromResult(data);
    //     }
    //
    //     public Expression<Func<IEnumerable<decimal>>> ReadRangeExpr => () => ReadRangeLogic();
    //    
    //     private IEnumerable<decimal> ReadRangeLogic()
    //     {
    //         while (!Stack.IsEmpty)
    //         {
    //             var success = Stack.TryPop(out var data);
    //
    //             if (success)
    //             {
    //                 yield return data;
    //             }
    //         }
    //     }
    //
    //     public bool CanExecute { get; set; } = true;
    // }
}