using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpGlide.Providers;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.TunnelWrappers.Performance
{
    public class MeasureConsumeSpeedConsumeWrapper<T> : IConsumeWrapper<T>
    {
        private readonly Stopwatch _sw = new Stopwatch();

        private readonly List<Metric> _data = new List<Metric>();

        public SpeedMetric GetMetric() => new SpeedMetric(_data.ToArray());

        public Func<IConsumeRoute, T> Wrap(Func<IConsumeRoute, T> funcToWrap)
        {
            return consumeRoute =>
            {
                _sw.Restart();

                var dataToReturn = funcToWrap(consumeRoute);
                
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