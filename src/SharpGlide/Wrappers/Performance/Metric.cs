using System;

namespace SharpGlide.Wrappers.Performance
{
    public struct Metric : IEquatable<Metric>
    {
        public bool Equals(Metric other)
        {
            return TimestampUtc.Equals(other.TimestampUtc) && MetricValue.Equals(other.MetricValue);
        }

        public override bool Equals(object obj)
        {
            return obj is Metric other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimestampUtc, MetricValue);
        }

        public static bool operator ==(Metric left, Metric right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Metric left, Metric right)
        {
            return !left.Equals(right);
        }

        public DateTime TimestampUtc { get; set; }

        public double MetricValue { get; set; }
    }
}