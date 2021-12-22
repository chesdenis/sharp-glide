using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public class PublishRouteAssignment<TConsumeData, TPublishData> : IPublishRouteAssignment<TConsumeData, TPublishData>
    {
        public PublishRouteAssignment(VectorPart<TConsumeData, TPublishData> part)
        {
            Part = part;
        }

        protected bool Equals(PublishRouteAssignment<TConsumeData, TPublishData> other)
        {
            return Part.Equals(other.Part);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PublishRouteAssignment<TConsumeData, TPublishData>)obj);
        }

        public override int GetHashCode()
        {
            return Part.GetHashCode();
        }

        public static bool operator ==(PublishRouteAssignment<TConsumeData, TPublishData> left, PublishRouteAssignment<TConsumeData, TPublishData> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PublishRouteAssignment<TConsumeData, TPublishData> left, PublishRouteAssignment<TConsumeData, TPublishData> right)
        {
            return !Equals(left, right);
        }

        public VectorPart<TConsumeData, TPublishData> Part { get; }

        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; } =
            new Dictionary<string, IPublishTunnel<TPublishData>>();
    }
}