using System;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.InMemory.Messaging;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Tunnels.InMemory
{
    // public class InMemoryPublishTunnel<T> : PublishTunnel<T>
    // {
    //     private readonly InMemoryBroker _broker;
    //     
    //     public InMemoryPublishTunnel(InMemoryBroker broker)
    //     {
    //         _broker = broker;
    //     }
    //
    //     public override Action<T, IPublishRoute> PublishPointer()
    //     {
    //         return (data, publishRoute) =>
    //         {
    //             foreach (var inMemoryQueue in _broker.EnumerateQueues(publishRoute))
    //             {
    //                 inMemoryQueue.Enqueue(data);
    //             }
    //         };
    //     }
    //
    //     public override void SetupInfrastructure(IPublishRoute publishRoute)
    //     {
    //         _broker.SetupInfrastructure(publishRoute);
    //     }
    // }
}