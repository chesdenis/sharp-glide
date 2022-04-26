using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpGlide.Tunnels.Readers;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    // public class PagedReaderExample : IPagedReader<decimal>
    // {
    //     public bool CanExecute { get; set; } = true;
    //     
    //     public readonly List<decimal> LargeList = new();
    //
    //     public Expression<Action<Action<IEnumerable<decimal>, PageInfo>, PageInfo>> ReadPageExpr
    //         => (action, info) => ReadPageLogic(action, info);
    //
    //     private void ReadPageLogic(Action<IEnumerable<decimal>, PageInfo> action, PageInfo info)
    //     {
    //         var itemsToSkip = info.PageIndex * info.PageSize;
    //         var itemsToTake = info.PageSize;
    //         
    //         if (itemsToSkip >= LargeList.Count)
    //         {
    //             action(LargeList, info);
    //             return;
    //         }
    //
    //         action(LargeList.Skip(itemsToSkip).Take(itemsToTake), info);
    //     }
    // }
}