using System;
using System.Collections.Generic;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public abstract class PublishTunnel<T> : IPublishTunnel<T>
    {
        public IList<IPublishWrapper<T>> OnPublishWrappers { get; } = new List<IPublishWrapper<T>>();

        public IPublishRoute PublishRoute { get; set; }

        protected readonly IPublishRoute DefaultRoute = new PublishRoute
        {
            Topic = $"{typeof(T)}_default",
            RoutingKey = "#"
        };

        public abstract Action<T, IPublishRoute> PublishPointer();
        
        public void Publish(T data)
        {
            var resultAction = PublishPointer();

            var childRoute = GetPublishRoute().CreateChild();
            
            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction);
            }

            resultAction(data, childRoute);
        }

        public void Publish(T data, string routingKey)
        {
            var resultAction = PublishPointer();

            var childRoute = GetPublishRoute().CreateChild(routingKey);
            
            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction);
            }

            resultAction(data, childRoute);
        }
        
        public void Publish(T data, IPublishRoute publishRoute)
        {
            var resultAction = PublishPointer();

            foreach (var wrapper in OnPublishWrappers)
            {
                resultAction = wrapper.Wrap(resultAction);
            }

            resultAction(data, publishRoute);
        }

        public abstract void SetupInfrastructure(IPublishRoute publishRoute);

        private IPublishRoute GetPublishRoute()
        {
            return PublishRoute ?? DefaultRoute;
        }
    }
}