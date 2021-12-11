using System.Collections.Generic;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public interface IPublishTunnel<T>
    {
        IList<IPublishWrapper<T>> OnPublishWrappers { get; }

        IPublishRoute PublishRoute { get; set; }

        void Publish(T data);
        void Publish(T data, string routingKey);
        void Publish(T data, IPublishRoute publishRoute);

        void SetupInfrastructure(IPublishRoute publishRoute);
    }
}