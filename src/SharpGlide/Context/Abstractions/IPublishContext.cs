using System.Collections.Generic;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Context.Abstractions
{
    public interface IPublishContext<TPublishData>
    { 
        IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; }

        TPublishWrapper AddPublishWrapper<TPublishWrapper>(TPublishWrapper wrapper)
            where TPublishWrapper : IPublishWrapper<TPublishData>;

        void SetupBindingToTopic(IPublishTunnel<TPublishData> tunnel, IPublishRoute publishRoute);
        void Publish(TPublishData data, IPublishRoute publishRoute);
        void Publish(TPublishData data);
        void Publish(TPublishData data, string routingKey);
    }
}