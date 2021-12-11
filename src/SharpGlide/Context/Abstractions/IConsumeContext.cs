using System.Collections.Generic;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Context.Abstractions
{
    public interface IConsumeContext<TConsumeData>
    {
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }

        void SetupBindingToTopic(IConsumeTunnel<TConsumeData> tunnel, IConsumeRoute consumeRoute);
        
        IEnumerable<TConsumeData> ReadAndConsumeData();
        void Consume(TConsumeData data);
        void Consume(TConsumeData data, string tunnelKey);
        void ConsumeRange(IEnumerable<TConsumeData> data);
    }
}