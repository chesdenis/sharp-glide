using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpGlide.Providers;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.TunnelWrappers.Performance
{
    public class MeasureConsumeSpeedConsumeWrapper<T> : IConsumeWrapper<T>
    {
        private readonly Stopwatch _sw = new Stopwatch();

        private readonly List<Metric> _data   = new List<Metric>();

        public SpeedMetric GetMetric() => new SpeedMetric(_data.ToArray());

        public Func<string, string, string, T> Wrap(Func<string, string, string, T> funcToWrap)
        {
            return (topicName, queueName, routingKeys) =>
            {
                _sw.Reset();
                _sw.Start();

                var dataToReturn = funcToWrap(topicName, queueName, routingKeys);
                
                _sw.Stop();
                
                _data.Add(new Metric
                {
                    TimestampUtc = DateTimeProvider.NowUtc,
                    MetricValue = _sw.Elapsed.TotalMilliseconds
                });
                
                return dataToReturn;
            };
        }
    }
}