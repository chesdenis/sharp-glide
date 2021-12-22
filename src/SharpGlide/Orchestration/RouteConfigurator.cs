using System.Collections.Generic;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public abstract class RouteConfigurator
    {
        protected readonly List<RouteLink> RouteLinks;
        protected readonly string DefaultQueue;
        protected readonly string DefaultRoutingKey;
        protected readonly string DefaultTopic;

        protected RouteConfigurator(
            List<RouteLink> routeLinks, 
            string defaultQueue,
            string defaultRoutingKey,
            string defaultTopic)
        {
            RouteLinks = routeLinks;
            DefaultQueue = defaultQueue;
            DefaultRoutingKey = defaultRoutingKey;
            DefaultTopic = defaultTopic;
        }
    }
}