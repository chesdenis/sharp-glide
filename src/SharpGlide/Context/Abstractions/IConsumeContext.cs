using System.Collections.Generic;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Context.Abstractions
{
    public interface IConsumeContext<TConsumeData>
    {
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }

        TConsumeWrapper AddConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IConsumeWrapper<TConsumeData>;

        void SetupBindingToTopic(IConsumeTunnel<TConsumeData> tunnel, IConsumeRoute consumeRoute);
        
        IEnumerable<TConsumeData> ReadAndConsumeData();
        void Push(TConsumeData data);
        void Push(TConsumeData data, string tunnelKey);
        void PushRange(IEnumerable<TConsumeData> data);
    }
}