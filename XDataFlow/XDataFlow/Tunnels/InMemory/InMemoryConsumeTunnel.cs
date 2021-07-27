using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        
        private int _previousWaitingToConsume = 0;
        private DateTime _previousWaitingToConsumeDateTime = DateTime.Now;
        
        public override int WaitingToConsume
        {
            get
            {
                var waitingToConsume = _broker.FindQueues(TopicName, RoutingKey)
                    .Select(inMemoryQueue => inMemoryQueue.Count).Sum();

                return waitingToConsume;
            }
        }

        public override int EstimatedTimeInSeconds
        {
            get
            {
                var currentWaitingToConsume = WaitingToConsume;

                if (_previousWaitingToConsume > currentWaitingToConsume)
                {
                    var processedMessagesDelta = Math.Abs(_previousWaitingToConsume - currentWaitingToConsume);
                    if (processedMessagesDelta == 0)
                    {
                        return 0;
                    }
                    
                    var timeRangeDeltaInSeconds = DateTime.Now.Subtract(_previousWaitingToConsumeDateTime).TotalSeconds;

                    var secondsPerMessage = timeRangeDeltaInSeconds / processedMessagesDelta;
                    var estimatedTime = Convert.ToInt32(secondsPerMessage * currentWaitingToConsume);
                    this._messagesPerSecond = Convert.ToInt32(timeRangeDeltaInSeconds > 0 ? processedMessagesDelta / timeRangeDeltaInSeconds : 0);
                    
                    return estimatedTime;
                }
                
                _previousWaitingToConsume = currentWaitingToConsume;
                _previousWaitingToConsumeDateTime = DateTime.Now;

                return 0;
            }
        }

        private int _messagesPerSecond = 0;
        public override int MessagesPerSecond
        {
            get
            {
                return _messagesPerSecond;
            }
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