using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public abstract class TypedPublisherOnlyFlowPart<TIn> : DataFlowPart
    {
        public IList<ITypedPublishTunnel<TIn>> PublishTunnels { get; } = new List<ITypedPublishTunnel<TIn>>();
    }
}