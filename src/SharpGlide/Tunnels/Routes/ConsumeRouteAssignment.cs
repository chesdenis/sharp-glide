using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public class ConsumeRouteAssignment<TConsumeData, TPublishData> : IConsumeRouteAssignment<TConsumeData, TPublishData>
    {
        protected bool Equals(ConsumeRouteAssignment<TConsumeData, TPublishData> other)
        {
            return Part.Equals(other.Part);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConsumeRouteAssignment<TConsumeData, TPublishData>)obj);
        }

        public override int GetHashCode()
        {
            return Part.GetHashCode();
        }

        public static bool operator ==(ConsumeRouteAssignment<TConsumeData, TPublishData> left, ConsumeRouteAssignment<TConsumeData, TPublishData> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConsumeRouteAssignment<TConsumeData, TPublishData> left, ConsumeRouteAssignment<TConsumeData, TPublishData> right)
        {
            return !Equals(left, right);
        }

        public ConsumeRouteAssignment(VectorPart<TConsumeData, TPublishData> part)
        {
            Part = part;
        }

        public VectorPart<TConsumeData, TPublishData> Part { get; }

        public IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; } =
            new Dictionary<string, IConsumeTunnel<TConsumeData>>();
    }
}