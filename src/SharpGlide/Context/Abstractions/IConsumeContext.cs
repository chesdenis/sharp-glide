using System.Collections.Generic;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Context.Abstractions
{
    public interface IConsumeContext<TConsumeData>
    {
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }

        void SetupBindingToTopic(IConsumeTunnel<TConsumeData> tunnel, IConsumeRoute consumeRoute);
        
        IEnumerable<TConsumeData> Consume();


        void TakeAndConsume<TConsumeRoute>(
            TConsumeRoute consumeRoute, 
            params TConsumeData[] data) 
            where TConsumeRoute : IConsumeRoute;

        void TakeAndConsume<TConsumeRoute>(
            string tunnelKey, 
            TConsumeRoute consumeRoute, 
            params TConsumeData[] data)
            where TConsumeRoute : IConsumeRoute;
    }
}