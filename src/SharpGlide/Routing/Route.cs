using System;

namespace SharpGlide.Routing
{
    public class Route : IRoute
    {
        protected bool Equals(Route other)
        {
            return RoutingKey == other.RoutingKey && Queue == other.Queue && Topic == other.Topic;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Route)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RoutingKey, Queue, Topic);
        }

        public static bool operator ==(Route left, Route right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Route left, Route right)
        {
            return !Equals(left, right);
        }

        public static Route Default => new Route
        {
            RoutingKey = "#",
            Queue = ".",
            Topic = "."
        };

        public override string ToString()
        {
            return $"RoutingKey: {RoutingKey}, Queue: {Queue}, Topic: {Topic}";
        }

        public string RoutingKey { get; set; }
        public string Queue { get; set; }
        public string Topic { get; set; }
    }
}