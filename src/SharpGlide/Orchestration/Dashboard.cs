using System.Collections.Generic;
using System.Linq;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public class Dashboard : IDashboard
    {
        private readonly IDashboardSelection _dashboardSelection;
        private const string DefaultQueue = "Default";
        private const string DefaultRoutingKey = "#";
        private const string DefaultTopic = "#";

        private readonly List<RouteLink> _routesLinks = new List<RouteLink>();
        private readonly IConsumeRouteConfigurator<IXConsumeRoute> _consumeRouteConfigurator;
        private readonly IPublishRouteConfigurator<IXPublishRoute> _publishRouteConfigurator;

        private Dashboard(IDashboardSelection dashboardSelection)
        {
            _dashboardSelection = dashboardSelection;
            
            _consumeRouteConfigurator = new ConsumeRouteConfigurator<IXConsumeRoute>(_routesLinks,
                DefaultQueue,
                DefaultRoutingKey,
                DefaultTopic);
            
            _publishRouteConfigurator = new PublishRouteConfigurator<IXPublishRoute>(_routesLinks, DefaultQueue,
                DefaultRoutingKey,
                DefaultTopic);
        }

        public static IDashboard Create() => new Dashboard
        (
            new DashboardSelection()
        );

        public IList<IBasePart> Selection => _dashboardSelection.Selection;

        
        public IEnumerable<RouteLink> EnumerateRoutes() => _routesLinks.AsEnumerable();
        public IEnumerable<IBasePart> EnumerateSelection() => Selection.AsEnumerable();

        public IConsumeRouteConfigurator<IXConsumeRoute> ConsumeFrom<TConsumeData, TPublishData>(string topic, string queue, string routingKey,
            VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            return _consumeRouteConfigurator.ConsumeFrom(topic, queue, routingKey, vectorPart);
        }

        public IConsumeRouteConfigurator<IXConsumeRoute> ConsumeFrom<TConsumeData, TPublishData>(string queue, string routingKey,
            VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            return _consumeRouteConfigurator.ConsumeFrom(queue, routingKey, vectorPart);
        }

        public IConsumeRouteConfigurator<IXConsumeRoute> ConsumeFrom<TConsumeData, TPublishData>(string queue, VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            return _consumeRouteConfigurator.ConsumeFrom(queue, vectorPart);
        }

        public IConsumeRouteConfigurator<IXConsumeRoute> ConsumeFrom<TConsumeData, TPublishData>(VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            return _consumeRouteConfigurator.ConsumeFrom(vectorPart);
        }

        public IPublishRouteConfigurator<IXPublishRoute> PublishTo<TConsumeData, TPublishData>(string topic, string routingKey, string queue,
            VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            return _publishRouteConfigurator.PublishTo(topic, routingKey, queue, vectorPart);
        }

        public IPublishRouteConfigurator<IXPublishRoute> PublishTo<TConsumeData, TPublishData>(string topic, string routingKey, VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            return _publishRouteConfigurator.PublishTo(topic, routingKey, vectorPart);
        }

        public IPublishRouteConfigurator<IXPublishRoute> PublishTo<TConsumeData, TPublishData>(string topic, VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            return _publishRouteConfigurator.PublishTo(topic, vectorPart);
        }

        public IPublishRouteConfigurator<IXPublishRoute> PublishTo<TConsumeData, TPublishData>(VectorPart<TConsumeData, TPublishData> vectorPart)
        {
            return _publishRouteConfigurator.PublishTo(vectorPart);
        }
    }
}