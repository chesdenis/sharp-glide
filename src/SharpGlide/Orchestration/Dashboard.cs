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

        public IConsumeRouteConfigurator<IXConsumeRoute> ConsumeFrom(string topic, string queue, string routingKey,
            IBasePart basePart) =>
            _consumeRouteConfigurator.ConsumeFrom(topic, queue, routingKey, basePart);

        public IConsumeRouteConfigurator<IXConsumeRoute> ConsumeFrom(string queue, string routingKey, IBasePart appliedPart) =>
            _consumeRouteConfigurator.ConsumeFrom(queue, routingKey, appliedPart);

        public IConsumeRouteConfigurator<IXConsumeRoute> ConsumeFrom(string queue, IBasePart appliedPart) =>
            _consumeRouteConfigurator.ConsumeFrom(queue, appliedPart);

        public IConsumeRouteConfigurator<IXConsumeRoute> ConsumeFrom(IBasePart appliedPart) => _consumeRouteConfigurator.ConsumeFrom(appliedPart);

        public IPublishRouteConfigurator<IXPublishRoute> PublishTo(string topic, string routingKey, string queue, IBasePart appliedPart) =>
            _publishRouteConfigurator.PublishTo(topic, routingKey, queue, appliedPart);

        public IPublishRouteConfigurator<IXPublishRoute> PublishTo(string topic, string routingKey, IBasePart appliedPart) =>
            _publishRouteConfigurator.PublishTo(topic, routingKey, appliedPart);

        public IPublishRouteConfigurator<IXPublishRoute> PublishTo(string topic, IBasePart appliedPart) =>
            _publishRouteConfigurator.PublishTo(topic, appliedPart);

        public IPublishRouteConfigurator<IXPublishRoute> PublishTo(IBasePart appliedPart) => _publishRouteConfigurator.PublishTo(appliedPart);
        
        public IEnumerable<RouteLink> EnumerateRoutes() => _routesLinks.AsEnumerable();
        public IEnumerable<IBasePart> EnumerateSelection() => Selection.AsEnumerable();
    }
}