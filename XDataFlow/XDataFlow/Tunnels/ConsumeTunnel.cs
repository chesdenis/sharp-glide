using System;
using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public abstract class ConsumeTunnel<T> : IConsumeTunnel<T>
    {
        public IList<IWrapperWithOutput<T>> OnConsumeWrappers { get; } = new List<IWrapperWithOutput<T>>();
        public List<string> RoutingKeys { get; } = new List<string>();
        public string TopicName { get; set; }
        public string QueueName { get; set; }

        public abstract Func<T> ConsumePointer();
        
        public T Consume()
        {
            var consumeFunc = ConsumePointer();

            foreach (var wrapper in OnConsumeWrappers)
            {
                consumeFunc = wrapper.Wrap(consumeFunc);
            }

            return consumeFunc();
        }

        public abstract void Put(T input);
    }
}