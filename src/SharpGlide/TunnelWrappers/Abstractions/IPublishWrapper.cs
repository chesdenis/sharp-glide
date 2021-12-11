using System;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.TunnelWrappers.Abstractions
{
    public interface IPublishWrapper<T>
    {
        Action<T, IPublishRoute> Wrap(Action<T, IPublishRoute> actionToWrap);
    }
}