using System;
using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public abstract class PublishTunnel<T> : IPublishTunnel<T>
    {
        public IList<IWrapperWithInput<T>> OnPublishWrappers { get; set; } = new List<IWrapperWithInput<T>>();
        public string TopicName { get; set; }
        public string RoutingKey { get; set; }

        public abstract Action<T, string, string> PublishPointer();
        
        public void Publish(T data)
        {
            var resultAction = PublishPointer();

            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction, TopicName, RoutingKey);
            }

            resultAction(data, TopicName, RoutingKey);
        }

        public void Publish(T data, string routingKey)
        {
            var resultAction = PublishPointer();

            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction, TopicName, routingKey);
            }

            resultAction(data, TopicName, routingKey);
        }

        public void Publish(T data, string exchange, string routingKey)
        {
            var resultAction = PublishPointer();

            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction, exchange, routingKey);
            }
            
            resultAction(data, exchange, routingKey);
        }

        public abstract void SetupInfrastructure(string topicName, string routingKey);
    }
}