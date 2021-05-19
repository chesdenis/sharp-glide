using System;

namespace XDataFlow.FlowConfig
{
    public class QueueConfigNode
    {
        public string Name { get; set; }

        protected bool Equals(QueueConfigNode other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((QueueConfigNode) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name) : 0);
        }

        public static bool operator ==(QueueConfigNode left, QueueConfigNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(QueueConfigNode left, QueueConfigNode right)
        {
            return !Equals(left, right);
        }
    }
}