using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public abstract class ConsumerOnlyFlowPart<TOut> : DataFlowPart
    {
        public IList<IConsumeTunnel<TOut>> ConsumeTunnels { get; } = new List<IConsumeTunnel<TOut>>();
    }
}