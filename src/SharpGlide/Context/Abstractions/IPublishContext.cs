using System.Collections.Generic;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Context.Abstractions
{
    public interface IPublishContext<TPublishData>
    { 
        IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; }

        TPublishWrapper AddPublishWrapper<TPublishWrapper>(TPublishWrapper wrapper)
            where TPublishWrapper : IPublishWrapper<TPublishData>;

        void SetupBindingToTopic(IPublishTunnel<TPublishData> tunnel, string topicName, string routingKey);
    }
}