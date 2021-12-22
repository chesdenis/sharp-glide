using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Orchestration
{
    public interface IConsumeRouteConfigurator<TRoute> where TRoute : IXConsumeRoute
    {
        IConsumeRouteConfigurator<TRoute> ConsumeFrom(string topic, string queue, string routingKey, IBasePart appliedPart);
        IConsumeRouteConfigurator<TRoute> ConsumeFrom(string queue, string routingKey, IBasePart appliedPart);
        IConsumeRouteConfigurator<TRoute> ConsumeFrom(string queue, IBasePart appliedPart);
        IConsumeRouteConfigurator<TRoute> ConsumeFrom(IBasePart appliedPart);
    }
}