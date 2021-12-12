using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace SharpGlide.TunnelWrappers.Performance
{
    public abstract class MeasurePerformanceWrapper
    {
        protected readonly Stopwatch Sw = new Stopwatch();

        private readonly Timer _timer = new Timer(1000);

        private readonly List<Metric> _tempStorage = new List<Metric>();

        private readonly List<Metric> _snapshotStorage = new List<Metric>();

        public readonly List<PerformanceReport> PerformanceReports = new List<PerformanceReport>();

        private bool _isCalculating = false;

        protected MeasurePerformanceWrapper()
        {
            _timer.Elapsed += DoCalculation;
        }

        private void DoCalculation(object sender, ElapsedEventArgs e)
        {
            lock (_timer)
            {
                _isCalculating = true;

                var performanceReport = new PerformanceReport();
                performanceReport.PopulateReport(_snapshotStorage);
                PerformanceReports.Add(performanceReport);
                _snapshotStorage.Clear();
                
                _isCalculating = false;
            }
        }
        
        protected void StoreMetric(Metric metric)
        {
            if (_isCalculating)
            {
                _tempStorage.Add(metric);
            }
            else
            {
                _snapshotStorage.Add(metric);

                if (_tempStorage.Count > 0)
                {
                    _snapshotStorage.AddRange(_tempStorage);
                    _tempStorage.Clear();
                }
            }
        }
    }
}