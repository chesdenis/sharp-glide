using System;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IXConsumeRoute
    {
    }

    public struct XConsumeRoute : IXConsumeRoute
    {
        public bool Equals(XConsumeRoute other)
        {
            return 
                   string.Equals(RoutingKey, other.RoutingKey, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Queue, other.Queue, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return obj is XConsumeRoute other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(RoutingKey, StringComparer.OrdinalIgnoreCase);
            hashCode.Add(Queue, StringComparer.OrdinalIgnoreCase);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(XConsumeRoute left, XConsumeRoute right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(XConsumeRoute left, XConsumeRoute right)
        {
            return !left.Equals(right);
        }
        
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
    }
}