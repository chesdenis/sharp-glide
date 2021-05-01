using System;
using System.Linq;
using XDataFlow.Parts;
using XDataFlow.Tunnels;

namespace XDataFlow.Extensions
{
    public static class PublishTunnelExtensions
    {
        public static void AddPublishTunnel<TTunnel, TIn>(this PublisherOnlyFlowPart<TIn> part, Func<TTunnel> tunnelPointer)
            where TTunnel : IPublishTunnel<TIn>
        {
            var tunnel = tunnelPointer();
            part.PublishTunnels.Add(tunnel);
        }
        
        public static void AddPublishTunnel<TTunnel, TIn, TOut>(this PublisherConsumerFlowPart<TIn, TOut> part, Func<TTunnel> tunnelPointer)
            where TTunnel : IPublishTunnel<TIn>
        {
            var tunnel = tunnelPointer();
            part.PublishTunnels.Add(tunnel);
        }
        
        public static void Publish<TDataFlowPart, TIn>(this TDataFlowPart flowPart, TIn data)
            where TDataFlowPart : PublisherOnlyFlowPart<TIn>
        {
            var tunnels =  flowPart.PublishTunnels.ToList();

            foreach (var t in tunnels)
            {
                t.Publish(data);
            }
        }
        
        public static void Publish<TDataFlowPart, TIn, TOut>(this TDataFlowPart flowPart, TIn data)
            where TDataFlowPart : PublisherConsumerFlowPart<TIn, TOut>
        {
            var tunnels =  flowPart.PublishTunnels.ToList();

            foreach (var t in tunnels)
            {
                t.Publish(data);
            }
        }
    }
}