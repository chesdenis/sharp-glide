using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Tunnels.Readers;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    // public class ReaderViaCallbackExample : IReaderViaCallback<decimal>
    // {
    //     public readonly ConcurrentStack<decimal> Stack = new();
    //         
    //     public Expression<Action<Action<decimal>>> ReadViaCallbackExpr =>
    //         consumeCallback => ConsumeWithCallbackLogic(consumeCallback);
    //
    //     private void ConsumeWithCallbackLogic(Action<decimal> consumeCallback)
    //     {
    //         while (!Stack.IsEmpty)
    //         {
    //             var success = Stack.TryPop(out var data);
    //
    //             if (success)
    //             {
    //                 consumeCallback?.Invoke(data);
    //             }
    //         }
    //     }
    //
    //     public Expression<Action<Action<IEnumerable<decimal>>>> ReadRangeViaCallbackExpr => 
    //         consumeCallback =>
    //             ConsumeRangeWithCallbackLogic(consumeCallback);
    //
    //     private void ConsumeRangeWithCallbackLogic(Action<IEnumerable<decimal>> consumeCallback)
    //     {
    //         while (!Stack.IsEmpty)
    //         {
    //             var success = Stack.TryPop(out var data);
    //
    //             if (success)
    //             {
    //                 consumeCallback?.Invoke(new List<decimal>() { data });
    //             }
    //         }
    //     }
    //
    //
    //     public bool CanExecute { get; set; } = true;
    // }
}