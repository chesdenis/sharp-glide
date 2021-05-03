using System;
using System.Linq;
using XDataFlow.Parts;
using XDataFlow.Tunnels;

namespace XDataFlow.Extensions
{
    public static class PublishTunnelExtensions
    {
        public static void AddPublishTunnel<TTunnel, TPublishData>(this IFlowPublisherPart<TPublishData> part, Func<TTunnel> tunnelPointer)
            where TTunnel : IPublishTunnel<TPublishData>
        {
            var tunnel = tunnelPointer();
            var key = typeof(TPublishData).FullName ?? throw new InvalidOperationException();
            part.PublishTunnels.Add(key, tunnel);
        }
        
        public static void Publish<TDataFlowPart, TPublishData>(this TDataFlowPart flowPart, TPublishData data)
            where TDataFlowPart : IFlowPublisherPart<TPublishData>
        {
            var tunnels = flowPart.PublishTunnels;

            foreach (var tunnelKey in tunnels.Keys)
            {
                tunnels[tunnelKey].Publish(data);
            }
        }
    }
}