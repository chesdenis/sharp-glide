using System;
using System.Collections.Generic;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public abstract class ConsumeTunnel<T> : IConsumeTunnel<T>
    {
        public IList<IConsumeWrapper<T>> OnConsumeWrappers { get; } = new List<IConsumeWrapper<T>>();
        public IDictionary<string, IConsumeRoute> Routes { get; set; } = new Dictionary<string, IConsumeRoute>();

        public abstract Func<IConsumeRoute, T> ConsumePointer();
        
        public T Consume(IConsumeRoute consumeRoute)
        {
            if (!Routes.ContainsKey(consumeRoute.Name))
            {
                throw new ArgumentOutOfRangeException(nameof(consumeRoute),
                    "Route was not registered. Please create to get correct visualization map");
            }
            
            var consumeFunc = ConsumePointer();

            foreach (var wrapper in OnConsumeWrappers)
            {
                consumeFunc = wrapper.Wrap(consumeFunc);
            }

            return consumeFunc(consumeRoute);
        }

        public abstract void TakeAndConsume(T input, IConsumeRoute consumeRoute);
        
        public abstract void SetupInfrastructure(IConsumeRoute consumeRoute);
    }
}