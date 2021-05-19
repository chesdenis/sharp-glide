using System;

namespace XDataFlow.FlowConfig
{
    public class RouteConfigNode
    {
        public TopicConfigNode TopicConfigNode { get; set; }

        public QueueConfigNode QueueConfigNode { get; set; }

        public string RoutingKey { get; set; }

        protected bool Equals(RouteConfigNode other)
        {
            return Equals(TopicConfigNode, other.TopicConfigNode) && Equals(QueueConfigNode, other.QueueConfigNode) &&
                   string.Equals(RoutingKey, other.RoutingKey, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RouteConfigNode) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(TopicConfigNode);
            hashCode.Add(QueueConfigNode);
            hashCode.Add(RoutingKey, StringComparer.InvariantCultureIgnoreCase);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(RouteConfigNode left, RouteConfigNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RouteConfigNode left, RouteConfigNode right)
        {
            return !Equals(left, right);
        }
    }
}