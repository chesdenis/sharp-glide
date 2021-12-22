using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Abstractions;
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

        public IPublishRouteConfigurator<TRoute> PublishTo<TConsumeData, TPublishData>(string topic, string routingKey, string queue, VectorPart<TConsumeData, TPublishData> vectorPart)
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
            
            Store(routeLink, vectorPart);
            return this;
        }

        public IPublishRouteConfigurator<TRoute> PublishTo<TConsumeData, TPublishData>(string topic, string routingKey,  VectorPart<TConsumeData, TPublishData> vectorPart) => PublishTo(topic, routingKey, DefaultQueue, vectorPart);
        public IPublishRouteConfigurator<TRoute> PublishTo<TConsumeData, TPublishData>(string topic,  VectorPart<TConsumeData, TPublishData> vectorPart) => PublishTo(topic, DefaultRoutingKey, vectorPart);
        public IPublishRouteConfigurator<TRoute> PublishTo<TConsumeData, TPublishData>( VectorPart<TConsumeData, TPublishData> vectorPart) => PublishTo(DefaultTopic, DefaultRoutingKey, vectorPart);
        
        private void Store<TConsumeData, TPublishData>(RouteLink routeLink, VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            var existedRoute = RouteLinks.FirstOrDefault(f =>
                string.Equals(
                    f.PublishRoute?.Topic,
                    routeLink.PublishRoute.Topic, StringComparison.OrdinalIgnoreCase)
                && string.Equals(
                    f.PublishRoute?.RoutingKey,
                    routeLink.PublishRoute.RoutingKey, StringComparison.OrdinalIgnoreCase));

            var routeAssignment = new PublishRouteAssignment<TConsumeData, TPublishData>(vectorPart);
            
            if (existedRoute != null)
            {
                existedRoute.PublishRoute.Assign(routeAssignment);
            }
            else
            {
                routeLink.PublishRoute.Assign(routeAssignment);
                RouteLinks.Add(routeLink);
            }
        }
    }
}