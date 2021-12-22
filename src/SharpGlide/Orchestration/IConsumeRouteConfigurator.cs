using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public interface IConsumeRouteConfigurator<TRoute> where TRoute : IXConsumeRoute
    {
        IConsumeRouteConfigurator<TRoute> ConsumeFrom<TConsumeData, TPublishData>(string topic, string queue, string routingKey, VectorPart<TConsumeData, TPublishData> vectorPart);
        IConsumeRouteConfigurator<TRoute> ConsumeFrom<TConsumeData, TPublishData>(string queue, string routingKey, VectorPart<TConsumeData, TPublishData> vectorPart);
        IConsumeRouteConfigurator<TRoute> ConsumeFrom<TConsumeData, TPublishData>(string queue, VectorPart<TConsumeData, TPublishData> vectorPart);
        IConsumeRouteConfigurator<TRoute> ConsumeFrom<TConsumeData, TPublishData>(VectorPart<TConsumeData, TPublishData> vectorPart);
    }
}