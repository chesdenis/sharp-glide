using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public interface IPublishTunnel<T>
    {
        IList<IWrapperWithInput<T>> OnPublishWrappers { get; }

        string TopicName { get; set; }

        void Publish(T data);

        void Publish(T data, string routingKey);
    }
}