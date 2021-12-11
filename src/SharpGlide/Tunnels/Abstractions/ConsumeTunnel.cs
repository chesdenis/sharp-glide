using System;
using System.Collections.Generic;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public abstract class ConsumeTunnel<T> : IConsumeTunnel<T>
    {
        public IList<IConsumeWrapper<T>> OnConsumeWrappers { get; } = new List<IConsumeWrapper<T>>();
        
        public IConsumeRoute ConsumeRoute { get; set; }

        public abstract Func<IConsumeRoute, T> ConsumePointer();
        
        public T Consume(IConsumeRoute consumeRoute)
        {
            var consumeFunc = ConsumePointer();

            foreach (var wrapper in OnConsumeWrappers)
            {
                consumeFunc = wrapper.Wrap(consumeFunc);
            }

            return consumeFunc(consumeRoute);
        }

        public T Consume() => Consume(ConsumeRoute);

        public abstract void Put(T input, IConsumeRoute consumeRoute);
        
        public void Put(T input) => Put(input, ConsumeRoute);
        public abstract void SetupInfrastructure(IConsumeRoute consumeRoute);
    }
}