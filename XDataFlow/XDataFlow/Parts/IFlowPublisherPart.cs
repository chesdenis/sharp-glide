using System;
using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public interface IFlowPublisherPart<TPublishData>
    {
        IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; }  
        
        void Publish(TPublishData data, Func<KeyValuePair<string, IPublishTunnel<TPublishData>>, bool> predicate);
        
        void Publish(TPublishData data);
    }
}