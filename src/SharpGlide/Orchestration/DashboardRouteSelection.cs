using System.Collections.Generic;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public class DashboardRouteSelection<TConsumeData, TPublishData>
        : IDashboardRouteSelection<TConsumeData, TPublishData>
    {
        public IEnumerable<IConsumeRouteAssignment<TConsumeData, TPublishData>> ConsumeAssignments { get; set; }
        public IEnumerable<IPublishRouteAssignment<TConsumeData, TPublishData>> PublishAssignments { get; set; }
        public IDashboard Dashboard { get; set; }
    }
}