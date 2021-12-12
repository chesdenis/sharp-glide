using System.Collections.Generic;

namespace SharpGlide.TunnelWrappers.Performance
{
    public class MetricSeries
    {
        public List<Metric> Metrics { get; set; } = new List<Metric>();
        public Granularity Granularity { get; set; }
    }
}