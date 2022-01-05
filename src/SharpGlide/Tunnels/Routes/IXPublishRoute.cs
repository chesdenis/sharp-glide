using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public interface IXPublishRoute
    {
        void Assign<TConsumeData, TPublishData>(IPublishRouteAssignment<TConsumeData, TPublishData> routeAssignment);
        string Topic { get; set; }
        string RoutingKey { get; set; }
    }
}