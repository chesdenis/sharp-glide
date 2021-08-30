using System;
using System.Collections.Generic;
using XDataFlow.Tunnels;
using XDataFlow.Wrappers;

namespace XDataFlow.Context
{
    public class PublishContext<TConsumeData, TPublishData> : IPublishContext<TPublishData>
    {
        private readonly IHeartBeatContext _heartBeatContext;

        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; } =
            new Dictionary<string, IPublishTunnel<TPublishData>>();

        public PublishContext(IHeartBeatContext heartBeatContext)
        {
            _heartBeatContext = heartBeatContext;
        }
        
        public TPublishWrapper AddPublishWrapper<TPublishWrapper>(TPublishWrapper wrapper)
            where TPublishWrapper : IWrapperWithInput<TPublishData>
        {
            foreach (var tunnelKey in this.PublishTunnels.Keys)
            {
                this.PublishTunnels[tunnelKey].OnPublishWrappers.Add(wrapper);
            }

            return wrapper;
        }
        
        public void SetupBindingToTopic(IPublishTunnel<TPublishData> tunnel, string topicName, string routingKey)
        {
            tunnel.RoutingKey = routingKey;
            tunnel.TopicName = topicName;

            tunnel.SetupInfrastructure(topicName, routingKey);
            
            this.PublishTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);
        }

        public void Publish(TPublishData data)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data);
                _heartBeatContext.LastPublishedAt = DateTime.Now;
            }
        }

        public void Publish(TPublishData data, string routingKey)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data, routingKey);
                _heartBeatContext.LastPublishedAt = DateTime.Now;
            }
        }

        public void Publish(TPublishData data, string topicName, string routingKey)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data, topicName, routingKey);
                _heartBeatContext.LastPublishedAt = DateTime.Now;
            }
        }
    }
}