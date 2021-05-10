using System;
using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public interface IFlowPublisherPart<TPublishData>
    {
        IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; }  
        
        void Publish(TPublishData data);

        void Publish(TPublishData data, string routingKey);

        void Publish(TPublishData data, string topicName, string routingKey);
    }
}