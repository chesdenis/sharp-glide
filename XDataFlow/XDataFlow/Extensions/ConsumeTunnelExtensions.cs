using System;
using XDataFlow.Parts;
using XDataFlow.Tunnels;

namespace XDataFlow.Extensions
{
    public static class ConsumeTunnelExtensions
    {
        public static void RegisterConsumeTunnel<TTunnel>(this BytesFlowPart part, Func<TTunnel> tunnelPointer)
            where TTunnel : IConsumeTunnel
        {
            var tunnel = tunnelPointer();
            part.ConsumeTunnels.Add(tunnel);
        }
        
        public static void RegisterConsumeTunnel<TTunnel, TOut>(this TypedConsumerOnlyFlowPart<TOut> part, Func<TTunnel> tunnelPointer)
            where TTunnel : ITypedConsumeTunnel<TOut>
        {
            var tunnel = tunnelPointer();
            part.ConsumeTunnels.Add(tunnel);
        }
        
        public static void RegisterConsumeTunnel<TTunnel, TIn, TOut>(this TypedPublisherConsumerFlowPart<TIn, TOut> part, Func<TTunnel> tunnelPointer)
            where TTunnel : ITypedConsumeTunnel<TOut>
        {
            var tunnel = tunnelPointer();
            part.ConsumeTunnels.Add(tunnel);
        }
        
        public static void Consume<TDataFlowPart>(this TDataFlowPart flowPart, Action<byte[]> onReceive)
            where TDataFlowPart : BytesFlowPart
        {
            var tunnels = flowPart.ConsumeTunnels;

            foreach (var t in tunnels)
            {
                onReceive(t.Consume());
            }
        }
        
        public static void Consume<TDataFlowPart, TIn, TOut>(this TDataFlowPart flowPart, Action<TOut> onReceive)
            where TDataFlowPart : TypedPublisherConsumerFlowPart<TIn, TOut>
        {
            var tunnels = flowPart.ConsumeTunnels;

            foreach (var t in tunnels)
            {
                onReceive(t.Consume());
            }
        }
        
        public static void Consume<TDataFlowPart, TOut>(this TDataFlowPart flowPart, Action<TOut> onReceive)
            where TDataFlowPart : TypedConsumerOnlyFlowPart<TOut>
        {
            var tunnels = flowPart.ConsumeTunnels;

            foreach (var t in tunnels)
            {
                onReceive(t.Consume());
            }
        }
    }
}