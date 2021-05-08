using System;
using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public abstract class PublishTunnel<T> : IPublishTunnel<T>
    {
        private const string DefaultRoutingKey = "#";
        
        public IList<IWrapperWithInput<T>> OnPublishWrappers { get; set; } = new List<IWrapperWithInput<T>>();
        public string TopicName { get; set; }

        public abstract Action<T, string> PublishPointer();
        
        public void Publish(T data)
        {
            var resultAction = PublishPointer();

            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction, DefaultRoutingKey);
            }

            resultAction(data, DefaultRoutingKey);
        }

        public void Publish(T data, string routingKey)
        {
            var resultAction = PublishPointer();

            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction, routingKey);
            }
            
            resultAction(data, routingKey);
        }
    }
}