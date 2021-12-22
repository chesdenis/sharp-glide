using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IXPublishRoute
    {
        List<IBasePart> AssignedParts { get; set; }
        string Topic { get; set; }
        string RoutingKey { get; set; }
    }
}