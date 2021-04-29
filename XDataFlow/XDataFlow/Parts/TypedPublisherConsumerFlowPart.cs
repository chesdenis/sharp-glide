using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public abstract class TypedPublisherConsumerFlowPart<TIn, TOut> : DataFlowPart
    {
        public IList<ITypedPublishTunnel<TIn>> PublishTunnels { get; } = new List<ITypedPublishTunnel<TIn>>();
        public IList<ITypedConsumeTunnel<TOut>> ConsumeTunnels { get; } = new List<ITypedConsumeTunnel<TOut>>();
    }
}