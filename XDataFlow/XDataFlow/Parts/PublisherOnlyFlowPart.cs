using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public abstract class PublisherOnlyFlowPart<TIn> : DataFlowPart
    {
        public IList<IPublishTunnel<TIn>> PublishTunnels { get; } = new List<IPublishTunnel<TIn>>();
    }
}