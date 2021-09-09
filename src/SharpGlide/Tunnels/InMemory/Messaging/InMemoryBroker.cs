using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpGlide.Tunnels.InMemory.Messaging
{
    public class InMemoryBroker
    {
        private static readonly Lazy<InMemoryBroker> LazyFactory =
            new Lazy<InMemoryBroker>(() => new InMemoryBroker());
    
        public static InMemoryBroker Current => LazyFactory.Value;

        private InMemoryBroker()
        {
        }
        
        public InMemoryTopics Topics { get; } = new InMemoryTopics();
        public InMemoryQueues Queues { get; } = new InMemoryQueues();
        public InMemoryRoutes Routes { get; } = new InMemoryRoutes();

        public void SetupInfrastructure(string topicName)
        {
            lock (Current)
            {
                Topics.Add(topicName);
            }
        }

        public void SetupInfrastructure(string topicName, string queueName, string routingKey)
        {
            lock (Current)
            {
                Topics.Add(topicName);
                Queues.TryAdd(queueName, new InMemoryQueue<object>());
                Routes.Add(new InMemoryRoute
                {
                    TopicName = topicName,
                    QueueName = queueName,
                    RoutingKey = routingKey
                });
            }
        }

        public IEnumerable<InMemoryQueue<object>> FindQueues(string topicName, string routingKey)
        {
            lock (Current)
            {
                var routes = Routes.Where(
                    w=> string.Equals(w.TopicName, topicName, StringComparison.InvariantCultureIgnoreCase)
                        && string.Equals(w.RoutingKey, routingKey, StringComparison.InvariantCultureIgnoreCase));

                foreach (var route in routes)
                {
                    yield return Queues[route.QueueName];
                }
            }
        }
    }
}