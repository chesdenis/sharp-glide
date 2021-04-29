using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public abstract class TypedConsumerOnlyFlowPart<TOut> : DataFlowPart
    {
        public IList<ITypedConsumeTunnel<TOut>> ConsumeTunnels { get; } = new List<ITypedConsumeTunnel<TOut>>();
    }
}