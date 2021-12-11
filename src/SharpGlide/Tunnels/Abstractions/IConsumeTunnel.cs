using System.Collections.Generic;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public interface IConsumeTunnel<T>
    {
        IList<IConsumeWrapper<T>> OnConsumeWrappers { get; }
        
        IConsumeRoute ConsumeRoute { get; set; }

        T Consume();

        T Consume(IConsumeRoute consumeRoute);
        
        void Put(T input, IConsumeRoute consumeRoute);
        
        void Put(T input);
        
        void SetupInfrastructure(IConsumeRoute consumeRoute);
    }
}