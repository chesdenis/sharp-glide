using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public interface IPublishTunnel<T>
    {
        IList<IWrapperWithInput<T>> OnPublishWrappers { get; }
        
        void Publish(T data);
    }
}