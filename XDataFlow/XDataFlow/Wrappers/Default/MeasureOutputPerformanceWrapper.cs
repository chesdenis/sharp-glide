using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace XDataFlow.Wrappers.Default
{
    public class MeasureOutputPerformanceWrapper<T> : IWrapperWithOutput<T>
    {
        private readonly Stopwatch _sw = new Stopwatch();
        
        public class PerformanceDataRow
        {
            public DateTime EventTimestamp { get; set; }

            public double TimeSpentInSeconds { get; set; }
        }

        private List<PerformanceDataRow> PerformanceData { get; set; } = new List<PerformanceDataRow>();

        public Func<T> Wrap(Func<T> funcToWrap)
        {
            return (() =>
            {
                _sw.Reset();
                _sw.Start();

                var dataToReturn = funcToWrap();
                
                _sw.Stop();
                
                PerformanceData.Add(new PerformanceDataRow()
                {
                    EventTimestamp = DateTime.Now,
                    TimeSpentInSeconds = _sw.Elapsed.TotalSeconds
                });
                
                return dataToReturn;
            });
        }
    }
}