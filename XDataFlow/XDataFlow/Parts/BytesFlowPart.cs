using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public abstract class BytesFlowPart : DataFlowPart
    {
        public IList<IPublishTunnel> PublishTunnels { get; } = new List<IPublishTunnel>();
        public IList<IConsumeTunnel> ConsumeTunnels { get; } = new List<IConsumeTunnel>();
    }
}