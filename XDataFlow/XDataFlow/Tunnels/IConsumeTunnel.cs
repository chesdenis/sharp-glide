using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public interface IConsumeTunnel<T>
    {
        IList<IWrapperWithOutput<T>> OnConsumeWrappers { get; }
        
        string RoutingKey { get; set; }

        string TopicName { get; set; }

        string QueueName { get; set; }

        T Consume();

        T Consume(string topicName, string queueName, string routingKey);
        
        void Put(T input, string topicName, string queueName, string routingKey);
        
        void Put(T input);
        
        void SetupInfrastructure(string topicName, string queueName, string routingKey);
    }
}