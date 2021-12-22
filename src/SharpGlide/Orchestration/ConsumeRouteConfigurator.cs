using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Abstractions;
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
        
        public IConsumeRouteConfigurator<TRoute> ConsumeFrom<TConsumeData, TPublishData>(string topic, string queue, string routingKey,
            VectorPart<TConsumeData, TPublishData> vectorPart)
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

            Store(routeLink, vectorPart);

            return this;
        }

        public IConsumeRouteConfigurator<TRoute> ConsumeFrom<TConsumeData, TPublishData>(string queue, string routingKey, VectorPart<TConsumeData, TPublishData> vectorPart)
            => ConsumeFrom(DefaultTopic, queue, routingKey, vectorPart);

        public IConsumeRouteConfigurator<TRoute> ConsumeFrom<TConsumeData, TPublishData>(string queue, VectorPart<TConsumeData, TPublishData> vectorPart) =>
            ConsumeFrom(queue, DefaultRoutingKey, vectorPart);

        public IConsumeRouteConfigurator<TRoute> ConsumeFrom<TConsumeData, TPublishData>(VectorPart<TConsumeData, TPublishData> vectorPart) => ConsumeFrom(DefaultQueue, DefaultRoutingKey, vectorPart);

        private void Store<TConsumeData, TPublishData>(RouteLink routeLink, VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            var existedRoute = RouteLinks.FirstOrDefault(f =>
                string.Equals(
                    f.ConsumeRoute?.Queue,
                    routeLink.ConsumeRoute?.Queue, StringComparison.OrdinalIgnoreCase)
                && string.Equals(
                    f.ConsumeRoute?.RoutingKey,
                    routeLink.ConsumeRoute?.RoutingKey, StringComparison.OrdinalIgnoreCase));

            var consumeRouteAssignment = new ConsumeRouteAssignment<TConsumeData, TPublishData>(vectorPart);
            
            if (existedRoute != null)
            {
                existedRoute.ConsumeRoute.AssignRoute(consumeRouteAssignment);
            }
            else
            {
                routeLink.ConsumeRoute.AssignRoute(consumeRouteAssignment);
                RouteLinks.Add(routeLink);
            }
        }
    }
}