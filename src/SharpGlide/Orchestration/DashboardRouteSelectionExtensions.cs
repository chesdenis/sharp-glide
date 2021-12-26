using System.Linq;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public static class DashboardRouteSelectionExtensions
    {
        public static IDashboardRouteSelection<TConsumeData, TPublishData>
            SelectRouteAssignments<TConsumeData, TPublishData>(
                this IDashboard dashboard,
                params VectorPart<TConsumeData, TPublishData>[] parts)
        {
            var consumeAssignments = dashboard.EnumerateRoutes()
                .Where(w => w.ConsumeRoute?.RouteAssignments != null)
                .SelectMany(sm => sm.ConsumeRoute.RouteAssignments)
                .Select(sm => sm.Key)
                .OfType<IConsumeRouteAssignment<TConsumeData, TPublishData>>()
                .Where(w => parts.Contains(w.Part));

            var publishAssignments = dashboard.EnumerateRoutes()
                .Where(w => w.PublishRoute?.RouteAssignments != null)
                .SelectMany(sm => sm.PublishRoute.RouteAssignments)
                .Select(sm => sm.Key)
                .OfType<IPublishRouteAssignment<TConsumeData, TPublishData>>()
                .Where(w => parts.Contains(w.Part));

            return new DashboardRouteSelection<TConsumeData, TPublishData>
            {
                ConsumeAssignments = consumeAssignments,
                PublishAssignments = publishAssignments,
                Dashboard = dashboard
            };
        }
    }
}