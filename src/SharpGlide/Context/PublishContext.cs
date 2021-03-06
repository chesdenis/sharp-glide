using System;
using System.Collections.Generic;
using SharpGlide.Context.Abstractions;
using SharpGlide.Providers;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Context
{
    public class PublishContext<TPublishData> : IPublishContext<TPublishData>
    {
        private readonly IVectorHeartBeatContext _heartBeatContext;

        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; } =
            new Dictionary<string, IPublishTunnel<TPublishData>>();

        public PublishContext(IVectorHeartBeatContext heartBeatContext)
        {
            _heartBeatContext = heartBeatContext;
        }

        public void SetupBindingToTopic(IPublishTunnel<TPublishData> tunnel, IPublishRoute publishRoute)
        {
            tunnel.PublishRoute = publishRoute;

            tunnel.SetupInfrastructure(publishRoute);
            
            PublishTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);
        }

        public void Publish(TPublishData data, IPublishRoute publishRoute)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data, publishRoute);
                _heartBeatContext.LastPublishedAt = DateTimeProvider.Now;
            }
        }
         
        public void Publish(TPublishData data)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data);
                _heartBeatContext.LastPublishedAt = DateTimeProvider.Now;
            }
        }
        
        public void Publish(TPublishData data, string routingKey)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data, routingKey);
                _heartBeatContext.LastPublishedAt = DateTimeProvider.Now;
            }
        }
    }
}