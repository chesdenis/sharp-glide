using System;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.InMemory;
using SharpGlide.Tunnels.InMemory.Messaging;

namespace SharpGlide.Extensions
{
    public static class FlowConfigurationExtensions
    {
        public static void FlowFromSelf<TConsumeData, TPublishData>
            (this VectorPart<TConsumeData, TPublishData> sourcePart)
        {
            var linkId = Guid.NewGuid().ToString("N");
            var topicName = $"{linkId}:{sourcePart.Name}<-[manual]";
            var queueName = $"{linkId}:{sourcePart.Name}<-[manual]";
            var routingKey = "#";
            
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TConsumeData>(InMemoryBroker.Current);
            
            sourcePart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, topicName, queueName, routingKey);
        }
        
        public static void FlowTo<TTargetConsumeData, TTargetPublishData, TConsumeData, TPublishData>(
            this VectorPart<TConsumeData, TPublishData> sourcePart,
            VectorPart<TTargetConsumeData, TTargetPublishData> targetPart)
        {
            var linkId = Guid.NewGuid().ToString("N");
            var topicName = $"{linkId}:{sourcePart.Name}->{targetPart.Name}";
            var queueName = $"{linkId}:{sourcePart.Name}->{targetPart.Name}";
            var routingKey = "#";

            var inMemoryPublishTunnel = new InMemoryPublishTunnel<TPublishData>(InMemoryBroker.Current);
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TTargetConsumeData>(InMemoryBroker.Current);
            
            sourcePart.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, topicName, routingKey);
            targetPart.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, topicName, queueName, routingKey);
        }
        
        public static void FlowTo<TTargetConsumeData, TTargetPublishData, TConsumeData, TPublishData>(
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
        }

        public static void FlowFrom<TSourceConsumeData, TSourcePublishData, TConsumeData, TPublishData>(
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
        }
    }
}