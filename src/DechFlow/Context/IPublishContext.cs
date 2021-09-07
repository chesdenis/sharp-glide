using System.Collections.Generic;
using DechFlow.Tunnels;
using DechFlow.Wrappers;

namespace DechFlow.Context
{
    public interface IPublishContext<TPublishData>
    { 
        IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; }

        TPublishWrapper AddPublishWrapper<TPublishWrapper>(TPublishWrapper wrapper)
            where TPublishWrapper : IWrapperWithInput<TPublishData>;

        void SetupBindingToTopic(IPublishTunnel<TPublishData> tunnel, string topicName, string routingKey);
    }
}