using System.Collections.Generic;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public interface IDashboardRouteSelection<TConsumeData, TPublishData>
    {
        IEnumerable<IConsumeRouteAssignment<TConsumeData, TPublishData>> ConsumeAssignments { get; set; }
        IEnumerable<IPublishRouteAssignment<TConsumeData, TPublishData>> PublishAssignments { get; set; }
        IDashboard Dashboard { get; set; }
    }
}