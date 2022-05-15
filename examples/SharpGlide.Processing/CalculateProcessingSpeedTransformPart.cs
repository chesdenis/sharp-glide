using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts;

namespace SharpGlide.Processing
{
    public class CalculateProcessingSpeedTransformPart : ITransformPart<CalculateProcessingSpeedTransformPart.Metric,
        CalculateProcessingSpeedTransformPart.TimeAndSpeed>
    {
        public struct Metric
        {
            public double Current { get; set; }
            public double Total { get; set; }
            public double ElapsedMs { get; set; }
        }

        public struct TimeAndSpeed
        {
            public TimeAndSpeed(double sumOfMetric, double latestTimeMs, Metric metric)
            {
                Current = metric.Current;
                Total = metric.Total;

                var speedMs = latestTimeMs == 0 ? 0 : sumOfMetric / latestTimeMs;
                var speedSec = latestTimeMs == 0 ? 0 : (sumOfMetric * 1000.0) / (latestTimeMs);
                var available = Total - Current;

                SpeedMs = speedMs;
                SpeedSec = speedSec;
                TimeSpent = TimeSpan.FromMilliseconds(latestTimeMs);
                FinishIn = TimeSpan.FromMilliseconds(available / speedMs);
            }

            public double Current { get; }
            public double Total { get; }

            public double Progress => Total == 0 ? 0 : (1.0 - (Total - Current) / Total) * 100.0;
            public double SpeedMs { get; }
            public double SpeedSec { get; }
            public TimeSpan TimeSpent { get; }
            public TimeSpan FinishIn { get; }
        }

        public string Name { get; set; }

        private double _sumCurrent = 0;
        private double _elapsedTimeMs = 0;

        public Task<TimeAndSpeed> TransformAsync(Metric input, CancellationToken cancellationToken)
        {
            _sumCurrent += input.Current;
            _elapsedTimeMs = input.ElapsedMs;

            return Task.FromResult(new TimeAndSpeed(_sumCurrent, _elapsedTimeMs, input));
        }
    }
}