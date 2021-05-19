using System;

namespace XDataFlow.FlowConfig
{
    public class TopicConfigNode
    {
        public string Name { get; set; }

        protected bool Equals(TopicConfigNode other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TopicConfigNode) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name) : 0);
        }

        public static bool operator ==(TopicConfigNode left, TopicConfigNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TopicConfigNode left, TopicConfigNode right)
        {
            return !Equals(left, right);
        }
    }
}