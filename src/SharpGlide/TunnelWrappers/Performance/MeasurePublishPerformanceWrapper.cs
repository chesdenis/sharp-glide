using System;
using SharpGlide.Providers;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.TunnelWrappers.Performance
{
    // public class MeasurePublishPerformanceWrapper<T> : MeasurePerformanceWrapper, IPublishWrapper<T>
    // {
    //     public Action<T, IPublishRoute> Wrap(Action<T, IPublishRoute> actionToWrap)
    //     {
    //         return (arg, publishRoute) =>
    //         {
    //             Sw.Restart();
    //
    //             actionToWrap(arg, publishRoute);
    //
    //             Sw.Stop();
    //
    //             var metric = new Metric
    //             {
    //                 EventTimestamp = DateTimeProvider.NowUtc
    //             };
    //
    //             StoreMetric(metric);
    //         };
    //     }
    // }
}