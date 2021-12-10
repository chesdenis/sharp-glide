using System;
using System.Collections.Generic;
using SharpGlide.Context.Abstractions;
using SharpGlide.Tunnels;
using SharpGlide.Wrappers;

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
        
        public TPublishWrapper AddPublishWrapper<TPublishWrapper>(TPublishWrapper wrapper)
            where TPublishWrapper : IPublishWrapper<TPublishData>
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].OnPublishWrappers.Add(wrapper);
            }

            return wrapper;
        }
        
        public void SetupBindingToTopic(IPublishTunnel<TPublishData> tunnel, string topicName, string routingKey)
        {
            tunnel.RoutingKey = routingKey;
            tunnel.TopicName = topicName;

            tunnel.SetupInfrastructure(topicName, routingKey);
            
            PublishTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);
        }

        public void Publish(TPublishData data)
        {
            foreach (var t in PublishTunnels)
            {
                t.Value.Publish(data);
                _heartBeatContext.LastPublishedAt = DateTime.Now;
            }
        }

        public void Publish(TPublishData data, string routingKey)
        {
            foreach (var t in PublishTunnels)
            {
                t.Value.Publish(data, routingKey);
                _heartBeatContext.LastPublishedAt = DateTime.Now;
            }
        }

        public void Publish(TPublishData data, string topicName, string routingKey)
        {
            foreach (var t in PublishTunnels)
            {
                t.Value.Publish(data, topicName, routingKey);
                _heartBeatContext.LastPublishedAt = DateTime.Now;
            }
        }
    }
}