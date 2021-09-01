using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using XDataFlow.Renders;

namespace XDataFlow.Parts.NetCore.Renders
{
    public static class StatusRendersExtensions
    {
        // TODO: refactor this
        // public static void RedirectStatusToConsole(Timer timer, IEnumerable<IPart> parts)
        // {
        //     RenderToConsole(parts);
        //     timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
        //     timer.Start();
        //
        //     timer.Elapsed += (sender, eventArgs) => RenderToConsole(parts);
        // }

        // TODO: refactor this
        // private static void RenderToConsole(IEnumerable<IPart> parts)
        // {
        //     var statusTextBuilder = new StringBuilder();
        //
        //     foreach (var part in parts)
        //     {
        //         var statusInfo = part.GetStatusInfo();
        //         if (!statusInfo.Any())
        //         {
        //             continue;
        //         }
        //         
        //         statusTextBuilder.AppendLine(part.Name);
        //         var statusTable = ConsoleTable.FromDynamic(statusInfo);
        //         
        //         statusTextBuilder.AppendLine(statusTable.ToString());
        //     }
        //
        //     Console.Clear();
        //     Console.Write(statusTextBuilder.ToString());
        //     File.WriteAllText("PartStatus.txt", statusTextBuilder.ToString());
        // }
    }
}