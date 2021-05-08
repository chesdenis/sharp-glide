using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace XDataFlow.Wrappers.Default
{
    public class MeasureInputPerformanceWrapper<T> : IWrapperWithInput<T>
    {
        private readonly Stopwatch _sw = new Stopwatch();
        
        public class PerformanceDataRow
        {
            public DateTime EventTimestamp { get; set; }

            public double TimeSpentInSeconds { get; set; }
        }

        public List<PerformanceDataRow> PerformanceData { get; set; } = new List<PerformanceDataRow>();
        
        public Action<T, string> Wrap(Action<T, string> actionToWrap, string routingKey)
        {
            return (arg, key) =>
            {
                _sw.Reset();
                _sw.Start();
                
                actionToWrap(arg, key);
                
                _sw.Stop();
                
                PerformanceData.Add(new PerformanceDataRow()
                {
                    EventTimestamp = DateTime.Now,
                    TimeSpentInSeconds = _sw.Elapsed.TotalSeconds
                });
            };
        }
    }
}