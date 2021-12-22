using System.Collections;
using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public interface IDashboard : IDashboardSelection,
        IConsumeRouteConfigurator<IXConsumeRoute>,
        IPublishRouteConfigurator<IXPublishRoute>
    {
        IEnumerable<RouteLink> EnumerateRoutes();

        IEnumerable<IBasePart> EnumerateSelection();
    }
}