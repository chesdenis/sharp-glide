using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpGlide.Providers;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.TunnelWrappers.Performance
{
    public class MeasurePublishSpeedPublishWrapper<T> : IPublishWrapper<T>
    {
        private readonly Stopwatch _sw = new Stopwatch();

        private readonly List<Metric> _data = new List<Metric>();
        
        public SpeedMetric GetMetric() => new SpeedMetric(_data.ToArray());

        public Action<T, IPublishRoute> Wrap(Action<T, IPublishRoute> actionToWrap)
        {
            return (arg, publishRoute) =>
            {
                _sw.Restart();

                actionToWrap(arg, publishRoute);

                _sw.Stop();

                _data.Add(new Metric
                {
                    TimestampUtc = DateTimeProvider.NowUtc,
                    MetricValue = _sw.Elapsed.TotalMilliseconds
                });
            };
        }
    }
}