using System;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IXPublishRoute
    {
    }

    public struct XPublishRoute : IXPublishRoute
    {
        public bool Equals(XPublishRoute other)
        {
            return
                string.Equals(Topic, other.Topic, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(RoutingKey, other.RoutingKey, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return obj is XPublishRoute other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Topic, StringComparer.OrdinalIgnoreCase);
            hashCode.Add(RoutingKey, StringComparer.OrdinalIgnoreCase);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(XPublishRoute left, XPublishRoute right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(XPublishRoute left, XPublishRoute right)
        {
            return !left.Equals(right);
        }

        public string Topic { get; set; }

        public string RoutingKey { get; set; }
    }
}