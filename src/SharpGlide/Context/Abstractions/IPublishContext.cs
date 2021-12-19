using System.Collections.Generic;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Context.Abstractions
{
    public interface IPublishContext<TPublishData>
    { 
        IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; }

        void SetupBindingToTopic(IPublishTunnel<TPublishData> tunnel, IPublishRoute publishRoute);
        
        void Publish(TPublishData data, IPublishRoute publishRoute);
    }
}