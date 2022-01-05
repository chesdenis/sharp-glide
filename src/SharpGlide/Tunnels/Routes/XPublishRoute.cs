using System;
using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public class XPublishRoute : IXPublishRoute
    {
        public override string ToString()
        {
            return $"{nameof(Topic)}: {Topic}, {nameof(RoutingKey)}: {RoutingKey}, {nameof(RouteAssignments)}: {RouteAssignments.Count}";
        }

        public readonly IDictionary<object, Type> RouteAssignments = new Dictionary<object, Type>();

        public void Assign<TConsumeData, TPublishData>(
            IPublishRouteAssignment<TConsumeData, TPublishData> routeAssignment)
        {
            if (RouteAssignments.ContainsKey(routeAssignment))
            {
                return;
            }
            
            RouteAssignments.Add(routeAssignment, routeAssignment.GetType());
        }
        
        public string Topic { get; set; }

        public string RoutingKey { get; set; }
    }
}