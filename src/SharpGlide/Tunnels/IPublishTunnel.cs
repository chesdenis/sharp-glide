using System.Collections.Generic;
using SharpGlide.Wrappers;

namespace SharpGlide.Tunnels
{
    public interface IPublishTunnel<T>
    {
        IList<IWrapperWithInput<T>> OnPublishWrappers { get; }

        string TopicName { get; set; }

        string RoutingKey { get; set; }

        void Publish(T data);
        
        void Publish(T data, string routingKey);

        void Publish(T data, string exchange, string routingKey);
        
        void SetupInfrastructure(string topicName, string routingKey);
    }
}