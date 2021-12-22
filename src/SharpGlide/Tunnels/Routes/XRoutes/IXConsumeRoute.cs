using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IXConsumeRoute
    {
        List<IBasePart> AssignedParts { get; set; }
        string RoutingKey { get; set; }
        string Queue { get; set; }
    }
}