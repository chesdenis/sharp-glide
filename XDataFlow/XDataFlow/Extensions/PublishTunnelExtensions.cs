using System;
using System.Linq;
using XDataFlow.Parts;
using XDataFlow.Tunnels;

namespace XDataFlow.Extensions
{
    public static class PublishTunnelExtensions
    {
        public static void RegisterPublishTunnel<TTunnel, TIn>(this TypedPublisherOnlyFlowPart<TIn> part, Func<TTunnel> tunnelPointer)
            where TTunnel : ITypedPublishTunnel<TIn>
        {
            var tunnel = tunnelPointer();
            part.PublishTunnels.Add(tunnel);
        }
        
        public static void RegisterPublishTunnel<TTunnel, TIn, TOut>(this TypedPublisherConsumerFlowPart<TIn, TOut> part, Func<TTunnel> tunnelPointer)
            where TTunnel : ITypedPublishTunnel<TIn>
        {
            var tunnel = tunnelPointer();
            part.PublishTunnels.Add(tunnel);
        }
        
        public static void RegisterPublishTunnel<TTunnel>(this BytesFlowPart part, Func<TTunnel> tunnelPointer)
            where TTunnel : IPublishTunnel
        {
            var tunnel = tunnelPointer();
            part.PublishTunnels.Add(tunnel);
        }
        
        public static void Publish<TDataFlowPart>(this TDataFlowPart flowPart, byte[] data)
            where TDataFlowPart : BytesFlowPart
        {
            var tunnels =  flowPart.PublishTunnels
                .Where(w => w.CanPublishThis(data)).ToList();

            foreach (var t in tunnels)
            {
                t.Publish(data);
            }
        }
        
        public static void Publish<TDataFlowPart, TIn>(this TDataFlowPart flowPart, TIn data)
            where TDataFlowPart : TypedPublisherOnlyFlowPart<TIn>
        {
            var tunnels =  flowPart.PublishTunnels
                .Where(w => w.CanPublishThis(data)).ToList();

            foreach (var t in tunnels)
            {
                t.Publish(data);
            }
        }
        
        public static void Publish<TDataFlowPart, TIn, TOut>(this TDataFlowPart flowPart, TIn data)
            where TDataFlowPart : TypedPublisherConsumerFlowPart<TIn, TOut>
        {
            var tunnels =  flowPart.PublishTunnels
                .Where(w => w.CanPublishThis(data)).ToList();

            foreach (var t in tunnels)
            {
                t.Publish(data);
            }
        }
    }
}