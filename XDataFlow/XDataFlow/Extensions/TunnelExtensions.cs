using System;
using System.Collections.Generic;
using System.Linq;
using XDataFlow.Parts;
using XDataFlow.Tunnels;

namespace XDataFlow.Extensions
{
    public static class TunnelExtensions
    {
        public static void RegisterPublishTunnel<TTunnel>(this IDataFlowPart part)
            where TTunnel : IPublishTunnel, new()
        {
            part.PublishTunnels.Add(new TTunnel());
        }
        
        public static void RegisterPublishTunnel<TTunnel>(this IDataFlowPart part, Func<TTunnel> tunnelPointer)
            where TTunnel : IPublishTunnel
        {
            var tunnel = tunnelPointer();
            part.PublishTunnels.Add(tunnel);
        }

        public static void RegisterConsumeTunnel<TTunnel>(this IDataFlowPart part, Func<TTunnel> tunnelPointer)
            where TTunnel : IConsumeTunnel
        {
            var tunnel = tunnelPointer();
            part.ConsumeTunnels.Add(tunnel);
        }
        
        public static void Publish<TDataFlowPart>(this TDataFlowPart flowPart, byte[] data)
            where TDataFlowPart : IDataFlowPart
        {
            var tunnels =  flowPart.PublishTunnels
                .Where(w => w.CanPublishThis(data)).ToList();

            foreach (var t in tunnels)
            {
                t.Publish(data);
            }
        }
        
        public static void Receive<TDataFlowPart>(this TDataFlowPart flowPart, Action<byte[]> onReceive)
            where TDataFlowPart : IDataFlowPart
        {
            var tunnels = flowPart.ConsumeTunnels;

            foreach (var t in tunnels)
            {
                onReceive(t.Receive());
            }
        }
    }
}