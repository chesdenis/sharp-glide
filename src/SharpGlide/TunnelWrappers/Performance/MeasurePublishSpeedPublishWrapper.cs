using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpGlide.Providers;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.TunnelWrappers.Performance
{
    public class MeasurePublishSpeedPublishWrapper<T> : IPublishWrapper<T>
    {
        private readonly Stopwatch _sw = new Stopwatch();

        private readonly List<Metric> _data = new List<Metric>();
        
        public SpeedMetric GetMetric() => new SpeedMetric(_data.ToArray());

        public Action<T, string, string> Wrap(Action<T, string, string> actionToWrap, string exchange,
            string routingKey)
        {
            return (arg, topicName, key) =>
            {
                _sw.Reset();
                _sw.Start();

                actionToWrap(arg, topicName, key);

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