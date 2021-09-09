using System;
using SharpGlide.Tunnels.InMemory.Messaging;

namespace SharpGlide.Tunnels.InMemory
{
    public class InMemoryPublishTunnel<T> : PublishTunnel<T>
    {
        private readonly InMemoryBroker _broker;
        
        public InMemoryPublishTunnel(InMemoryBroker broker)
        {
            _broker = broker;
        }

        public override Action<T, string, string> PublishPointer()
        {
            return (data, topicName, routingKey) =>
            {
                _broker.SetupInfrastructure(topicName);
                foreach (var inMemoryQueue in _broker.FindQueues(topicName, routingKey))
                {
                    inMemoryQueue.Enqueue(data);
                }
            };
        }

        public override void SetupInfrastructure(string topicName, string routingKey)
        {
            _broker.SetupInfrastructure(topicName);
        }
    }
}