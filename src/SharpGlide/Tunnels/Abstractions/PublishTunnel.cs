using System;
using System.Collections.Generic;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public abstract class PublishTunnel<T> : IPublishTunnel<T>
    {
        public IList<IPublishWrapper<T>> OnPublishWrappers { get; } = new List<IPublishWrapper<T>>();
      
        public bool CanExecute { get; set; }

        public IDictionary<string, IPublishRoute> Routes { get; set; } = new Dictionary<string, IPublishRoute>();

        public abstract Action<T, IPublishRoute> PublishPointer();

        public void Publish(T data, IPublishRoute publishRoute)
        {
            if (!Routes.ContainsKey(publishRoute.Name))
            {
                throw new ArgumentOutOfRangeException(nameof(publishRoute),
                    "Route was not registered. Please create to get correct visualization map");
            }
            
            var resultAction = PublishPointer();

            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction);
            }

            resultAction(data, publishRoute);
        }

        public abstract void SetupInfrastructure(IPublishRoute publishRoute);
    }
}