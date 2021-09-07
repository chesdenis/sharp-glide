using System.Collections.Generic;
using DechFlow.Wrappers;

namespace DechFlow.Tunnels
{
    public interface IConsumeTunnel<T>
    {
        IList<IWrapperWithOutput<T>> OnConsumeWrappers { get; }
        
        string RoutingKey { get; set; }

        string TopicName { get; set; }

        string QueueName { get; set; }

        int WaitingToConsume { get; }
        
        int EstimatedTimeInSeconds { get; }

        int MessagesPerSecond { get; }

        T Consume();

        T Consume(string topicName, string queueName, string routingKey);
        
        void Put(T input, string topicName, string queueName, string routingKey);
        
        void Put(T input);
        
        void SetupInfrastructure(string topicName, string queueName, string routingKey);
    }
}