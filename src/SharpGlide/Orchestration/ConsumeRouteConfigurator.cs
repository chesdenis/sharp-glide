using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public class ConsumeRouteConfigurator<TRoute> : RouteConfigurator, IConsumeRouteConfigurator<TRoute>
        where TRoute : IXConsumeRoute
    {
        public ConsumeRouteConfigurator(List<RouteLink> routeLinks,
            string defaultQueue,
            string defaultRoutingKey,
            string defaultTopic)
            : base(routeLinks, defaultQueue, defaultRoutingKey, defaultTopic)
        {
        }


        public IConsumeRouteConfigurator<TRoute> ConsumeFrom(string topic, string queue, string routingKey,
            IBasePart basePart)
        {
            var routeLink = new RouteLink
            {
                ConsumeRoute = new XConsumeRoute
                {
                    RoutingKey = routingKey,
                    Queue = queue
                },
                PublishRoute = RouteLinks.FirstOrDefault(f =>
                        string.Equals(f.PublishRoute?.Topic,
                            topic, StringComparison.OrdinalIgnoreCase))?.PublishRoute
            };

            Store(routeLink, basePart);

            return this;
        }

        public IConsumeRouteConfigurator<TRoute> ConsumeFrom(string queue, string routingKey, IBasePart appliedPart)
            => ConsumeFrom(DefaultTopic, queue, routingKey, appliedPart);

        public IConsumeRouteConfigurator<TRoute> ConsumeFrom(string queue, IBasePart appliedPart) =>
            ConsumeFrom(queue, DefaultRoutingKey, appliedPart);

        public IConsumeRouteConfigurator<TRoute> ConsumeFrom(IBasePart appliedPart) => ConsumeFrom(DefaultQueue, DefaultRoutingKey, appliedPart);

        private void Store(RouteLink routeLink, IBasePart basePart)
        {
            var existedRoute = RouteLinks.FirstOrDefault(f =>
                string.Equals(
                    f.ConsumeRoute?.Queue,
                    routeLink.ConsumeRoute?.Queue, StringComparison.OrdinalIgnoreCase)
                && string.Equals(
                    f.ConsumeRoute?.RoutingKey,
                    routeLink.ConsumeRoute?.RoutingKey, StringComparison.OrdinalIgnoreCase));

            if (existedRoute != null)
            {
                existedRoute.ConsumeRoute.AssignedParts.Add(basePart);
            }
            else
            {
                routeLink.ConsumeRoute.AssignedParts.Add(basePart);
                RouteLinks.Add(routeLink);
            }
        }
    }
}