using System;
using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public abstract class PublishTunnel<T> : IPublishTunnel<T>
    {
        public IList<IWrapperWithInput<T>> OnPublishWrappers { get; set; } = new List<IWrapperWithInput<T>>();
        
        public abstract Action<T> PublishPointer();
        
        public void Publish(T data)
        {
            var resultAction = PublishPointer();

            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction);
            }

            resultAction(data);
        }
    }
}