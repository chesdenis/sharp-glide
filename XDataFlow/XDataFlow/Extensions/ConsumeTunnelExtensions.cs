using System;
using XDataFlow.Parts;

namespace XDataFlow.Extensions
{
    public static class ConsumeTunnelExtensions
    {
        public static void Consume<TDataFlowPart, TConsumeData>(this TDataFlowPart flowPart, Action<TConsumeData> onReceive)
            where TDataFlowPart : IFlowConsumerPart<TConsumeData>
        {
            var tunnels = flowPart.ConsumeTunnels;

            foreach (var tunnelKey in tunnels.Keys)
            {
                onReceive(tunnels[tunnelKey].Consume());
            }
        }
    }
}