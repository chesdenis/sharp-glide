using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Tunnels.Readers;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    // public class ReaderViaEventExample : IReaderViaEvent<decimal>
    // {
    //     public readonly ConcurrentStack<decimal> Stack = new();
    //
    //     public event EventHandler<decimal> OnRead;
    //     public event EventHandler<IEnumerable<decimal>> OnReadRange;
    //
    //     public Expression<Action> ReadViaEventExpr => () => ConsumeViaEventLogic();
    //
    //     private void ConsumeViaEventLogic()
    //     {
    //         while (!Stack.IsEmpty)
    //         {
    //             var success = Stack.TryPop(out var data);
    //
    //             if (success)
    //             {
    //                 OnRead?.Invoke(this, data);
    //             }
    //         }
    //     }
    //
    //
    //     public Expression<Action> ReadRangeViaEventCallbackExpr => () => ConsumeRangeViaEventLogic();
    //
    //     private void ConsumeRangeViaEventLogic()
    //     {
    //         var range = new List<decimal>();
    //         while (!Stack.IsEmpty)
    //         {
    //             var success = Stack.TryPop(out var data);
    //
    //             if (success)
    //             {
    //                 range.Add(data);
    //             }
    //         }
    //
    //         OnReadRange?.Invoke(this, range);
    //     }
    //
    //     public bool CanExecute { get; set; } = true;
    // }
}