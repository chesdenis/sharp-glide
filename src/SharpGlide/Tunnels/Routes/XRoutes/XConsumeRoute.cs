using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public class XConsumeRoute : IXConsumeRoute
    {
        public override string ToString()
        {
            return
                $"{nameof(RoutingKey)}: {RoutingKey}, {nameof(Queue)}: {Queue}, {nameof(RouteAssignments)}: {RouteAssignments.Count}";
        }

        public readonly IDictionary<object, Type> RouteAssignments = new Dictionary<object, Type>();

        public void AssignRoute<TConsumeData, TPublishData>(
            IConsumeRouteAssignment<TConsumeData, TPublishData> routeAssignment)
        {
            RouteAssignments.Add(routeAssignment, routeAssignment.GetType());
        }

        public string RoutingKey { get; set; }
        public string Queue { get; set; }
    }
}