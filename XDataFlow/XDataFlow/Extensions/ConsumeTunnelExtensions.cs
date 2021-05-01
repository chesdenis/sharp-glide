using System;
using XDataFlow.Parts;
using XDataFlow.Tunnels;

namespace XDataFlow.Extensions
{
    public static class ConsumeTunnelExtensions
    {
        public static void AddConsumeTunnel<TTunnel, TOut>(this ConsumerOnlyFlowPart<TOut> part, Func<TTunnel> tunnelPointer)
            where TTunnel : IConsumeTunnel<TOut>
        {
            var tunnel = tunnelPointer();
            part.ConsumeTunnels.Add(tunnel);
        }
        
        public static void AddConsumeTunnel<TTunnel, TIn, TOut>(this PublisherConsumerFlowPart<TIn, TOut> part, Func<TTunnel> tunnelPointer)
            where TTunnel : IConsumeTunnel<TOut>
        {
            var tunnel = tunnelPointer();
            part.ConsumeTunnels.Add(tunnel);
        }
        
        public static void Consume<TDataFlowPart, TIn, TOut>(this TDataFlowPart flowPart, Action<TOut> onReceive)
            where TDataFlowPart : PublisherConsumerFlowPart<TIn, TOut>
        {
            var tunnels = flowPart.ConsumeTunnels;

            foreach (var t in tunnels)
            {
                onReceive(t.Consume());
            }
        }
        
        public static void Consume<TDataFlowPart, TOut>(this TDataFlowPart flowPart, Action<TOut> onReceive)
            where TDataFlowPart : ConsumerOnlyFlowPart<TOut>
        {
            var tunnels = flowPart.ConsumeTunnels;

            foreach (var t in tunnels)
            {
                onReceive(t.Consume());
            }
        }
    }
}