using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IPublishRouteAssignment<TConsumeData, TPublishData>
    {
        VectorPart<TConsumeData, TPublishData> Part { get; }
        IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; }
    }
}