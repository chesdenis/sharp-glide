using System;
using SharpGlide.Exceptions;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.InMemory.Messaging;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Tunnels.InMemory
{
    public class InMemoryConsumeTunnel<T> : ConsumeTunnel<T>
    {
        private readonly InMemoryBroker _broker;

        public InMemoryConsumeTunnel(InMemoryBroker broker)
        {
            _broker = broker;
        }

        public override Func<IConsumeRoute, T> ConsumePointer()
        {
            return (consumeRoute) =>
            {
                _broker.SetupInfrastructure(consumeRoute);

                foreach (var inMemoryQueue in _broker.EnumerateQueues(consumeRoute))
                {
                    if (inMemoryQueue.TryDequeue(out var result))
                    {
                        return (T) result;
                    }
                }

                throw new NoDataException();
            };
        }

        public override void Put(T input, IConsumeRoute consumeRoute)
        {
            _broker.SetupInfrastructure(consumeRoute);
            
            foreach (var inMemoryQueue in _broker.EnumerateQueues(consumeRoute))
            {
                inMemoryQueue.Enqueue(input);
            }
        }

        public override void SetupInfrastructure(IConsumeRoute consumeRoute)
        {
            _broker.SetupInfrastructure(consumeRoute);
        }
    }
}