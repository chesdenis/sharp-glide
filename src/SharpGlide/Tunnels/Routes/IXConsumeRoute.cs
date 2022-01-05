using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IXConsumeRoute
    {
        string RoutingKey { get; set; }
        string Queue { get; set; }
        void AssignRoute<TConsumeData, TPublishData>(IConsumeRouteAssignment<TConsumeData, TPublishData> routeAssignment);
    }
}