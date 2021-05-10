using System;
using XDataFlow.Exceptions;
using XDataFlow.Tunnels.InMemory.Messaging;

namespace XDataFlow.Tunnels.InMemory
{
    public class InMemoryConsumeTunnel<T> : ConsumeTunnel<T>
    {
        private readonly InMemoryBroker _broker;

        public InMemoryConsumeTunnel(InMemoryBroker broker)
        {
            _broker = broker;
        }

        public override Func<string, string, string, T> ConsumePointer()
        {
            return (topicName, queueName, routingKey) =>
            {
                _broker.SetupInfrastructure(topicName, queueName, routingKey);

                foreach (var inMemoryQueue in _broker.FindQueues(topicName, routingKey))
                {
                    if (inMemoryQueue.TryDequeue(out var result))
                    {
                        return (T) result;
                    }
                }

                throw new NoDataException();
            };
        }
        
        public override void Put(T input, string topicName, string queueName, string routingKey)
        {
            _broker.SetupInfrastructure(topicName, queueName, routingKey);
            
            foreach (var inMemoryQueue in _broker.FindQueues(topicName, routingKey))
            {
                inMemoryQueue.Enqueue(input);
            }
        }

        public override void SetupInfrastructure(string topicName, string queueName, string routingKey)
        {
            _broker.SetupInfrastructure(topicName, queueName, routingKey);
        }
    }
}