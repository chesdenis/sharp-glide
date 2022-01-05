using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IConsumeRouteAssignment<TConsumeData, TPublishData>
    {
        VectorPart<TConsumeData, TPublishData> Part { get; }
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }
    }
}