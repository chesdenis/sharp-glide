using System;
using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public interface IConsumeTunnel<T>
    {
        IList<IWrapperWithOutput<T>> OnConsumeWrappers { get; }
        
        List<string> RoutingKeys  { get; }

        string TopicName { get; set; }

        string QueueName { get; set; }

        T Consume();
        
        void Put(T input);
    }
}