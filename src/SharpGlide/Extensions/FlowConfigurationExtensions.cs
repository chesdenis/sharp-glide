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
            Action<IList<IConsumeWrapper<TConsumeData>>> configureWrappers = null)
        {
            var linkId = Guid.NewGuid().ToString("N");
            var topicName = $"{linkId}:{sourcePart.Name}<-[manual]";
            var queueName = $"{linkId}:{sourcePart.Name}<-[manual]";
            var routingKey = "#";

            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TConsumeData>(InMemoryBroker.Current);

            configureWrappers?.Invoke(inMemoryConsumeTunnel.OnConsumeWrappers);

            var consumeRoute = new ConsumeRoute()
            {
                Topic = topicName,
                Queue = queueName,
                RoutingKey = routingKey
            };
            
            sourcePart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, consumeRoute);

            return sourcePart;
        }

        public static VectorPart<TTargetConsumeData, TTargetPublishData> FlowTo<TTargetConsumeData, TTargetPublishData,
            TConsumeData, TPublishData>(
            this VectorPart<TConsumeData, TPublishData> sourcePart,
            VectorPart<TTargetConsumeData, TTargetPublishData> targetPart,
            Action<IList<IPublishWrapper<TPublishData>>> configureInputWrappers = null,
            Action<IList<IConsumeWrapper<TTargetConsumeData>>> configureOutputWrappers = null)
        {
            var linkId = Guid.NewGuid().ToString("N");
            var topicName = $"{linkId}:{sourcePart.Name}->{targetPart.Name}";
            var queueName = $"{linkId}:{sourcePart.Name}->{targetPart.Name}";
            var routingKey = "#";

            var inMemoryPublishTunnel = new InMemoryPublishTunnel<TPublishData>(InMemoryBroker.Current);
            configureInputWrappers?.Invoke(inMemoryPublishTunnel.OnPublishWrappers);
          
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TTargetConsumeData>(InMemoryBroker.Current);
            configureOutputWrappers?.Invoke(inMemoryConsumeTunnel.OnConsumeWrappers);
            
            var publishRoute = new PublishRoute()
            {
                Topic = topicName,
                RoutingKey = routingKey
            };
            var consumeRoute = new ConsumeRoute()
            {
                Topic = topicName,
                Queue = queueName,
                RoutingKey = routingKey
            };
            
            sourcePart.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, publishRoute);
            targetPart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, consumeRoute);

            return targetPart;
        }

        public static VectorPart<TTargetConsumeData, TTargetPublishData> FlowTo<TTargetConsumeData, TTargetPublishData,
            TConsumeData, TPublishData>(
            this VectorPart<TConsumeData, TPublishData> sourcePart,
            VectorPart<TTargetConsumeData, TTargetPublishData> targetPart, string routingKey)
        {
            var linkId = Guid.NewGuid().ToString("N");
            var topicName = $"{linkId}:{sourcePart.Name}->{targetPart.Name}";
            var queueName = $"{linkId}:{sourcePart.Name}->{targetPart.Name}";

            var inMemoryPublishTunnel = new InMemoryPublishTunnel<TPublishData>(InMemoryBroker.Current);
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TTargetConsumeData>(InMemoryBroker.Current);

            var publishRoute = new PublishRoute()
            {
                Topic = topicName,
                RoutingKey = routingKey
            };
            var consumeRoute = new ConsumeRoute()
            {
                Topic = topicName,
                Queue = queueName,
                RoutingKey = routingKey
            };
            
            sourcePart.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, publishRoute);
            targetPart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, consumeRoute);

            return targetPart;
        }

        public static VectorPart<TSourceConsumeData, TSourcePublishData> FlowFrom<TSourceConsumeData,
            TSourcePublishData, TConsumeData, TPublishData>(
            this VectorPart<TConsumeData, TPublishData> targetPart,
            VectorPart<TSourceConsumeData, TSourcePublishData> sourcePart)
        {
            var linkId = Guid.NewGuid().ToString("N");
            var topicName = $"{linkId}:{sourcePart.Name}->{targetPart.Name}";
            var queueName = $"{linkId}:{sourcePart.Name}->{targetPart.Name}";
            var routingKey = "#";

            var inMemoryPublishTunnel = new InMemoryPublishTunnel<TSourcePublishData>(InMemoryBroker.Current);
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TConsumeData>(InMemoryBroker.Current);
            
            var publishRoute = new PublishRoute()
            {
                Topic = topicName,
                RoutingKey = routingKey
            };
            
            var consumeRoute = new ConsumeRoute()
            {
                Topic = topicName,
                Queue = queueName,
                RoutingKey = routingKey
            };

            targetPart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, consumeRoute);
            sourcePart.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, publishRoute);

            return sourcePart;
        }
    }
}