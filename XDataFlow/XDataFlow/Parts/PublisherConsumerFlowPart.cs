using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public abstract class PublisherConsumerFlowPart<TIn, TOut> : DataFlowPart
    {
        public IList<IPublishTunnel<TIn>> PublishTunnels { get; } = new List<IPublishTunnel<TIn>>();
        public IList<IConsumeTunnel<TOut>> ConsumeTunnels { get; } = new List<IConsumeTunnel<TOut>>();
    }
}