using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public class PublishRouteConfigurator<TRoute> :RouteConfigurator, IPublishRouteConfigurator<TRoute> where TRoute : IXPublishRoute
    {
        public PublishRouteConfigurator(List<RouteLink> routeLinks,
            string defaultQueue, 
            string defaultRoutingKey,
            string defaultTopic) 
            : base(routeLinks, defaultQueue, defaultRoutingKey, defaultTopic)
        {
        }

        public IPublishRouteConfigurator<TRoute> PublishTo(string topic, string routingKey, string queue, IBasePart appliedPart)
        {
            var routeLink = new RouteLink
            {
                ConsumeRoute = RouteLinks.FirstOrDefault(f =>
                    string.Equals(f.ConsumeRoute?.Queue, 
                        queue, StringComparison.OrdinalIgnoreCase))?.ConsumeRoute,
                PublishRoute = new XPublishRoute
                {
                    Topic = topic,
                    RoutingKey = routingKey
                }
            };
            
            Store(routeLink, appliedPart);
            return this;
        }

        public IPublishRouteConfigurator<TRoute> PublishTo(string topic, string routingKey, IBasePart appliedPart) => PublishTo(topic, routingKey, DefaultQueue, appliedPart);
        public IPublishRouteConfigurator<TRoute> PublishTo(string topic, IBasePart appliedPart) => PublishTo(topic, DefaultRoutingKey, appliedPart);
        public IPublishRouteConfigurator<TRoute> PublishTo(IBasePart appliedPart) => PublishTo(DefaultTopic, DefaultRoutingKey, appliedPart);
        
        private void Store(RouteLink routeLink, IBasePart appliedPart)
        {
            var existedRoute = RouteLinks.FirstOrDefault(f =>
                string.Equals(
                    f.PublishRoute?.Topic,
                    routeLink.PublishRoute.Topic, StringComparison.OrdinalIgnoreCase)
                && string.Equals(
                    f.PublishRoute?.RoutingKey,
                    routeLink.PublishRoute.RoutingKey, StringComparison.OrdinalIgnoreCase));

            if (existedRoute != null)
            {
                existedRoute.PublishRoute.AssignedParts.Add(appliedPart);
            }
            else
            {
                routeLink.PublishRoute.AssignedParts.Add(appliedPart);
                RouteLinks.Add(routeLink);
            }
        }
    }
}