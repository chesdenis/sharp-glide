using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Tunnels.Routes;

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

        public void SetupInfrastructure(IPublishRoute publishRoute)
        {
            lock (Current)
            {
                Topics.Add(publishRoute.Topic);
            }
        }

        public void SetupInfrastructure(IConsumeRoute consumeRoute)
        {
            lock (Current)
            {
                Topics.Add(consumeRoute.Topic);
                Queues.TryAdd(consumeRoute.Queue, new InMemoryQueue<object>());
                Routes.Add(new InMemoryRoute
                {
                    TopicName = consumeRoute.Topic,
                    QueueName = consumeRoute.Queue,
                    RoutingKey = consumeRoute.RoutingKey
                });
            }
        }

        public IEnumerable<InMemoryQueue<object>> EnumerateQueues(IPublishRoute publishRoute)
        {
            return EnumerateQueues(publishRoute.Topic, publishRoute.RoutingKey);
        }
        
        public IEnumerable<InMemoryQueue<object>> EnumerateQueues(IConsumeRoute consumeRoute)
        {
            return EnumerateQueues(consumeRoute.Topic, consumeRoute.RoutingKey);
        }

        private IEnumerable<InMemoryQueue<object>> EnumerateQueues(string topic, string routingKey)
        {
            IEnumerable<InMemoryRoute> routes;
            
            lock (Current)
            {
                routes = Routes.Where(
                    w=> string.Equals(w.TopicName, topic, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(w.RoutingKey, routingKey, StringComparison.OrdinalIgnoreCase));
            }
            
            foreach (var route in routes)
            {
                yield return Queues[route.QueueName];
            }
        }
    }
}