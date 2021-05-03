using System;
using XDataFlow.Parts;
using XDataFlow.Tunnels;

namespace XDataFlow.Extensions
{
    public static class ConsumeTunnelExtensions
    {
        public static void AddConsumeTunnel<TTunnel, TConsumeData>(this IFlowConsumerPart<TConsumeData> part, Func<TTunnel> tunnelPointer)
            where TTunnel : IConsumeTunnel<TConsumeData>
        {
            var tunnel = tunnelPointer();
            var key = typeof(TConsumeData).FullName ?? throw new InvalidOperationException();
            part.ConsumeTunnels.Add(key, tunnel);
        }
        
        public static void AddConsumeTunnel<TTunnel, TConsumeData>(this IFlowConsumerPart<TConsumeData> part, string tunnelKey, Func<TTunnel> tunnelPointer)
            where TTunnel : IConsumeTunnel<TConsumeData>
        {
            var tunnel = tunnelPointer();
            part.ConsumeTunnels.Add(tunnelKey, tunnel);
        }
        
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