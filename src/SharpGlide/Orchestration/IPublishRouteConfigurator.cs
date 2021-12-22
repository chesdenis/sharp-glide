using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public interface IPublishRouteConfigurator<TRoute> where TRoute : IXPublishRoute
    {
        IPublishRouteConfigurator<TRoute> PublishTo(string topic, string routingKey, string queue, IBasePart appliedPart);
        IPublishRouteConfigurator<TRoute> PublishTo(string topic, string routingKey, IBasePart appliedPart);
        IPublishRouteConfigurator<TRoute> PublishTo(string topic, IBasePart appliedPart);
        IPublishRouteConfigurator<TRoute> PublishTo(IBasePart appliedPart);
    }
}