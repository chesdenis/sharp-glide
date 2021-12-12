using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Providers;

namespace SharpGlide.TunnelWrappers.Performance
{
    public class PerformanceReport
    {
        private readonly List<MetricSeries> _metrics = new List<MetricSeries>();

        public double CountLastSecond => GetLastMetrics(Granularity.Second).Count();
        public double CountLastMinute => GetLastMetrics(Granularity.Minute).Count();
        public double CountLastHour => GetLastMetrics(Granularity.Hourly).Count();
        public double CountLastDay => GetLastMetrics(Granularity.Daily).Count();

        public double AvgLastMs => GetSeries(Granularity.Ms).Average();
        public double AvgLastSecond => GetSeries(Granularity.Second).Average();
        public double AvgLastMinute => GetSeries(Granularity.Minute).Average();
        public double AvgLastHour => GetSeries(Granularity.Hourly).Average();
        public double AvgLastDay => GetSeries(Granularity.Daily).Average();
        
        public void PopulateReport(List<Metric> snapshotStorage)
        {
            var groupedByMs = snapshotStorage.GroupBy(g => g.EventTimestamp).ToList();
            var groupedBySecond = snapshotStorage
                .GroupBy(g => new DateTime(
                    g.EventTimestamp.Year, g.EventTimestamp.Month, g.EventTimestamp.Day,
                    g.EventTimestamp.Hour, g.EventTimestamp.Minute, g.EventTimestamp.Second)).ToList();

            var groupedByMinute = snapshotStorage
                .GroupBy(g => new DateTime(
                    g.EventTimestamp.Year, g.EventTimestamp.Month, g.EventTimestamp.Day,
                    g.EventTimestamp.Hour, g.EventTimestamp.Minute, 0)).ToList();

            var groupedByHour = snapshotStorage
                .GroupBy(g => new DateTime(
                    g.EventTimestamp.Year, g.EventTimestamp.Month, g.EventTimestamp.Day,
                    g.EventTimestamp.Hour, 0, 0)).ToList();

            var groupedByDay = snapshotStorage
                .GroupBy(g => new DateTime(
                    g.EventTimestamp.Year, g.EventTimestamp.Month, g.EventTimestamp.Day,
                    0, 0, 0)).ToList();

            _metrics.AddRange(GetSeries(groupedByMs, Granularity.Ms));
            _metrics.AddRange(GetSeries(groupedBySecond, Granularity.Second));
            _metrics.AddRange(GetSeries(groupedByMinute, Granularity.Minute));
            _metrics.AddRange(GetSeries(groupedByHour, Granularity.Hourly));
            _metrics.AddRange(GetSeries(groupedByDay, Granularity.Daily));
        }
        
        private IEnumerable<MetricSeries> GetSeries(List<IGrouping<DateTime, Metric>> groupedData, Granularity granularity)
        {
            return groupedData
                .Select(s => new MetricSeries
                {
                    Granularity = granularity,
                    Metrics = s
                        .Select(ss =>
                            new Metric()
                            {
                                EventTimestamp = ss.EventTimestamp
                            }).ToList()
                });
        }

        private IEnumerable<Metric> GetMetrics(Granularity granularity)
        {
            return _metrics
                .Where(w => w.Granularity == granularity)
                .SelectMany(ss => ss.Metrics);
        }

        private IEnumerable<long> GetSeries(Granularity granularity, int offset = -1)
        {
            return new List<long>()
            {
                GetLastMetrics(granularity).Count(),
                GetLastMetrics(granularity, offset).Count()
            };
        }

        private IEnumerable<Metric> GetLastMetrics(Granularity granularity, int offset = -1)
        {
            switch (granularity)
            {
                case Granularity.Ms:
                    return GetMetrics(granularity)
                        .Where(w => w.EventTimestamp >= DateTimeProvider.Now.AddMilliseconds(offset));
                case Granularity.Second:
                    return GetMetrics(granularity)
                        .Where(w => w.EventTimestamp >= DateTimeProvider.Now.AddSeconds(offset));
                case Granularity.Minute:
                    return GetMetrics(granularity)
                        .Where(w => w.EventTimestamp >= DateTimeProvider.Now.AddMinutes(offset));
                case Granularity.Hourly:
                    return GetMetrics(granularity)
                        .Where(w => w.EventTimestamp >= DateTimeProvider.Now.AddHours(offset));
                case Granularity.Daily:
                    return GetMetrics(granularity)
                        .Where(w => w.EventTimestamp >= DateTimeProvider.Now.AddDays(offset));
                default:
                    throw new ArgumentOutOfRangeException(nameof(granularity), granularity, null);
            }
        }
    }
}