using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Context.Abstractions;
using SharpGlide.Tunnels;
using SharpGlide.Wrappers;

namespace SharpGlide.Context
{
    public class ConsumeContext<TConsumeData> : IConsumeContext<TConsumeData>
    {
        public IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; } =
            new Dictionary<string, IConsumeTunnel<TConsumeData>>();
        
        public TConsumeWrapper AddConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IConsumeWrapper<TConsumeData>
        {
            foreach (var tunnelKey in ConsumeTunnels.Keys)
            {
                ConsumeTunnels[tunnelKey].OnConsumeWrappers.Add(wrapper);
            }
            
            return wrapper;
        }
        
        public void SetupBindingToTopic(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey)
        {
            tunnel.RoutingKey = routingKey;
            tunnel.QueueName = queueName;
            tunnel.TopicName = topicName;

            tunnel.SetupInfrastructure(topicName, queueName, routingKey);
            
            ConsumeTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);
        }
        
        public void Push(TConsumeData data)
        {
            ConsumeTunnels.First().Value.Put(data);
        }
        
        public void PushRange(IEnumerable<TConsumeData> data)
        {
            var firstConsumer = ConsumeTunnels.First().Value;
            foreach (var consumeData in data)
            {
                firstConsumer.Put(consumeData);
            }
        }

        public void Push(TConsumeData data, string tunnelKey)
        {
            ConsumeTunnels.First(f=>f.Key == tunnelKey).Value.Put(data);
        }
        
        // TODO: make it async
        public IEnumerable<TConsumeData> ReadAndConsumeData()
        {
            var tunnels = ConsumeTunnels;

            foreach (var tunnelKey in tunnels.Keys)
            {
                // TODO: make it awaitable
                var consumeData = tunnels[tunnelKey].Consume();
                yield return consumeData;
            }
        }
    }
}