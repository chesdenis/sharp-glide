using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace XDataFlow.Wrappers.Default
{
    public class MeasureConsumePerformanceWrapper<T> : IWrapperWithOutput<T>, IPerformanceMetrics
    {
        private readonly Stopwatch _sw = new Stopwatch();
        
        public class Metric
        {
            public DateTime Timestamp { get; set; }

            public double ValueInSeconds { get; set; }
        }

        private readonly List<Metric> _data   = new List<Metric>();
        
        public Func<string, string, string, T> Wrap(Func<string, string, string, T> funcToWrap)
        {
            return ((topicName, queueName, routingKeys) =>
            {
                _sw.Reset();
                _sw.Start();

                var dataToReturn = funcToWrap(topicName, queueName, routingKeys);
                
                _sw.Stop();
                
                _data.Add(new Metric()
                {
                    Timestamp = DateTime.Now,
                    ValueInSeconds = _sw.Elapsed.TotalSeconds
                });
                
                return dataToReturn;
            });
        }

        public double ItemsForLastSecond => _data.Count(w => w.Timestamp >= DateTime.Now.AddSeconds(-1));
        public double ItemsForLastMinute => _data.Count(w => w.Timestamp >= DateTime.Now.AddMinutes(-1));
        public double ItemsForLastHour => _data.Count(w => w.Timestamp >= DateTime.Now.AddHours(-1));

        public double AvgProcessingTimeInSecondsForLastMinute => _data
            .Where(w => w.Timestamp >= DateTime.Now.AddMinutes(-1))
            .Select(s => s.ValueInSeconds).Average();
    }
}