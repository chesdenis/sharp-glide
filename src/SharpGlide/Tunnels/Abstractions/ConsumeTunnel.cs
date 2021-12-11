using System;
using System.Collections.Generic;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public abstract class ConsumeTunnel<T> : IConsumeTunnel<T>
    {
        public IList<IConsumeWrapper<T>> OnConsumeWrappers { get; } = new List<IConsumeWrapper<T>>();
        public string RoutingKey { get; set; }
        public string TopicName { get; set; }
        public string QueueName { get; set; }

        public abstract Func<string, string, string, T> ConsumePointer();
        
        public T Consume(string topicName, string queueName, string routingKey)
        {
            var consumeFunc = ConsumePointer();

            foreach (var wrapper in OnConsumeWrappers)
            {
                consumeFunc = wrapper.Wrap(consumeFunc);
            }

            return consumeFunc(topicName, queueName, routingKey);
        }

        public T Consume() => Consume(TopicName, QueueName, RoutingKey);

        public abstract void Put(T input, string topicName, string queueName, string routingKey);
        
        public void Put(T input) => Put(input, TopicName, QueueName, RoutingKey);
        public abstract void SetupInfrastructure(string topicName, string queueName, string routingKey);
    }
}