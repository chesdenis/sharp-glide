using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpGlide.Providers;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.TunnelWrappers.Performance
{
    // public class MeasureConsumePerformanceWrapper<T> : MeasurePerformanceWrapper, IConsumeWrapper<T>
    // {
    //     public Func<IConsumeRoute, T> Wrap(Func<IConsumeRoute, T> funcToWrap)
    //     {
    //         return consumeRoute =>
    //         {
    //             Sw.Restart();
    //
    //             var dataToReturn = funcToWrap(consumeRoute);
    //             
    //             Sw.Stop();
    //             
    //             var metric = new Metric
    //             {
    //                 EventTimestamp = DateTimeProvider.NowUtc
    //             };
    //
    //             StoreMetric(metric);
    //             
    //             return dataToReturn;
    //         };
    //     }
    // }
}