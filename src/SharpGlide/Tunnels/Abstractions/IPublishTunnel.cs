using System.Collections.Generic;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Tunnels.Abstractions
{
    public interface IPublishTunnel<T>
    {
        IList<IPublishWrapper<T>> OnPublishWrappers { get; }
        
        void Publish(T data, IPublishRoute publishRoute);

        void SetupInfrastructure(IPublishRoute publishRoute);
    }
}