using System;
using System.Collections.Generic;
using System.Linq;
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

       // private List<Tuple<int, DateTime>> WaitingToConsumeHistory = new List<Tuple<int, DateTime>>();
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
                    
                    return estimatedTime;
                }
                
                _previousWaitingToConsume = currentWaitingToConsume;
                _previousWaitingToConsumeDateTime = DateTime.Now;

                return 0;

                // var historySnapshot = WaitingToConsumeHistory.ToList().OrderBy(o => o.Item2).ToList();
                // if (historySnapshot.Count == 0)
                // {
                //     return 0;
                // }
                //
                // var last = historySnapshot.Last();
                // var first = historySnapshot.First();
                //
                // var deltaY = last.Item1 - first.Item1;
                // if (deltaY <= 0)
                // {
                //     return 0;
                // }
                //
                // var deltaX = last.Item2.Subtract(first.Item2);
                //
                // var estimatedTimeInSeconds = Convert.ToInt32((deltaX.TotalSeconds / deltaY * WaitingToConsume));
                //
                // return estimatedTimeInSeconds;
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