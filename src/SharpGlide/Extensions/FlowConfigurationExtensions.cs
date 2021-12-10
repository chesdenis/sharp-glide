using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels;
using SharpGlide.Tunnels.InMemory;
using SharpGlide.Tunnels.InMemory.Messaging;
using SharpGlide.Wrappers;
using SharpGlide.Wrappers.Performance;

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

            sourcePart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, topicName, queueName, routingKey);

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
            
            sourcePart.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, topicName, routingKey);
            targetPart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, topicName, queueName, routingKey);

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

            sourcePart.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, topicName, routingKey);
            targetPart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, topicName, queueName, routingKey);

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

            targetPart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, topicName, queueName, routingKey);
            sourcePart.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, topicName, routingKey);

            return sourcePart;
        }
    }
}