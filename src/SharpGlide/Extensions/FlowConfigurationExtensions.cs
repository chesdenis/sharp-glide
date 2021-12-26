using System;
using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.InMemory;
using SharpGlide.Tunnels.InMemory.Messaging;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Extensions
{
    public static class FlowConfigurationExtensions
    {
        public static VectorPart<TConsumeData, TPublishData> FlowFromSelf<TConsumeData, TPublishData>
        (this VectorPart<TConsumeData, TPublishData> sourcePart,
            Action<IList<IConsumeWrapper<TConsumeData>>> configureWrappers = null) =>
            FlowFromSelf(sourcePart, ConsumeRoute.Default, configureWrappers);
        
        public static VectorPart<TConsumeData, TPublishData> FlowFromSelf<TConsumeData, TPublishData>
        (this VectorPart<TConsumeData, TPublishData> sourcePart,
            IConsumeRoute consumeRoute,
            Action<IList<IConsumeWrapper<TConsumeData>>> configureWrappers = null)
        {
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TConsumeData>(InMemoryBroker.Current);
        
            configureWrappers?.Invoke(inMemoryConsumeTunnel.OnConsumeWrappers);
        
            sourcePart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, consumeRoute);
        
            return sourcePart;
        }
        
        public static VectorPart<TTargetConsumeData, TTargetPublishData> FlowTo<TTargetConsumeData, TTargetPublishData,
            TConsumeData, TPublishData>(
            this VectorPart<TConsumeData, TPublishData> sourcePart,
            VectorPart<TTargetConsumeData, TTargetPublishData> targetPart,
            IPublishRoute publishRoute,
            IConsumeRoute consumeRoute,
            Action<IList<IConsumeWrapper<TTargetConsumeData>>> configureTargetConsumeWrappers = null,
            Action<IList<IPublishWrapper<TPublishData>>> configureSourcePublishWrappers = null)
        {
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TTargetConsumeData>(InMemoryBroker.Current);
            configureTargetConsumeWrappers?.Invoke(inMemoryConsumeTunnel.OnConsumeWrappers);
        
            var inMemoryPublishTunnel = new InMemoryPublishTunnel<TPublishData>(InMemoryBroker.Current);
            configureSourcePublishWrappers?.Invoke(inMemoryPublishTunnel.OnPublishWrappers);
            
            sourcePart.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, publishRoute);
            targetPart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, consumeRoute);
        
            return targetPart;
        }
    }
}