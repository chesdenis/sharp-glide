using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public interface IPublishRouteConfigurator<TRoute> where TRoute : IXPublishRoute
    {
        IPublishRouteConfigurator<TRoute> PublishTo<TConsumeData, TPublishData>(string topic, string routingKey, string queue, VectorPart<TConsumeData, TPublishData> vectorPart);
        IPublishRouteConfigurator<TRoute> PublishTo<TConsumeData, TPublishData>(string topic, string routingKey, VectorPart<TConsumeData, TPublishData> vectorPart);
        IPublishRouteConfigurator<TRoute> PublishTo<TConsumeData, TPublishData>(string topic, VectorPart<TConsumeData, TPublishData> vectorPart);
        IPublishRouteConfigurator<TRoute> PublishTo<TConsumeData, TPublishData>(VectorPart<TConsumeData, TPublishData> vectorPart);
    }
}