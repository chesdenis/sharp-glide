using System.Collections.Generic;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public interface IConsumeTunnel<T>
    {
        IList<IConsumeWrapper<T>> OnConsumeWrappers { get; }
        IDictionary<string, IConsumeRoute> Routes { get; set; }

        T Consume(IConsumeRoute consumeRoute);
        
        void TakeAndConsume(T input, IConsumeRoute consumeRoute);

        void SetupInfrastructure(IConsumeRoute consumeRoute);
    }
}