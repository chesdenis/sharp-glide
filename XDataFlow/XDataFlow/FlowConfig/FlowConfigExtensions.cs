using System;
using XDataFlow.Parts;
using XDataFlow.Tunnels;
using XDataFlow.Tunnels.InMemory;
using XDataFlow.Tunnels.InMemory.Messaging;

namespace XDataFlow.FlowConfig
{
    public static class FlowConfigExtensions
    {
        public static FlowConfig CreateConfig()
        {
            return new FlowConfig();
        }

        public static FlowConfig CreateTopic(this FlowConfig config, string topicName)
        {
            var topic = new TopicConfigNode() {Name = topicName};
            
            config.Topics.Add(topic);
            config.LastModifiedTopic = topic;
            
            return config;
        }
         
        public static FlowConfig ForRoute(this FlowConfig config, string routingKey)
        {
            var route = new RouteConfigNode() { RoutingKey = routingKey };
            
            config.LastModifiedRoute = route;

            return config;
        }
        
        public static FlowConfig CreateQueue(this FlowConfig config, string queueName = "")
        {
            queueName = string.IsNullOrWhiteSpace(queueName) ? Guid.NewGuid().ToString("N") : queueName;
            
            var queue = new QueueConfigNode() {Name = queueName};
            
            config.Queues.Add(queue);
           
            config.LastModifiedRoute.QueueConfigNode = queue;
            config.LastModifiedRoute.TopicConfigNode = config.LastModifiedTopic;

            config.Routes.Add(config.LastModifiedRoute);

            return config;
        }
        
        
        // TODO: implement more fluent way
        // public static FlowConfig AttachPublisher<TConsumeData, TPublishData, TPublishTunnel>(
        //     this FlowConfig config, 
        //     FlowPart<TConsumeData, TPublishData> part, TPublishTunnel publishTunnel)
        // where TPublishTunnel : PublishTunnel<TPublishData>
        // {
        //     part.ConfigurePublishing(
        //         publishTunnel,
        //         config.LastModifiedTopic.Name,
        //         config.LastModifiedRoute.RoutingKey);
        //    
        //     return config;
        // }
        //
        // public static FlowConfig AttachInMemoryPublisher<TConsumeData, TPublishData>( 
        //     this FlowConfig config,
        //     FlowPart<TConsumeData,TPublishData> part,
        //     InMemoryBroker broker)
        // {
        //     var tunnel = new InMemoryPublishTunnel<TPublishData>(broker);
        //
        //     return AttachPublisher(config, part, tunnel);
        // }
        //
        // public static FlowConfig AttachConsumer<TConsumeData, TPublishData, TConsumeTunnel>(
        //     this FlowConfig config, 
        //     FlowPart<TConsumeData,TPublishData> part, TConsumeTunnel consumeTunnel)
        //     where TConsumeTunnel : ConsumeTunnel<TConsumeData>
        // {
        //     part.ConfigureListening(consumeTunnel, 
        //         config.LastModifiedRoute.TopicConfigNode.Name,
        //         config.LastModifiedRoute.QueueConfigNode.Name,
        //         config.LastModifiedRoute.RoutingKey);
        //     
        //     return config;
        // }
        //
        // public static FlowConfig AttachInMemoryConsumer<TConsumeData, TPublishData>( 
        //     this FlowConfig config,
        //     FlowPart<TConsumeData,TPublishData> part,
        //     InMemoryBroker broker)
        // {
        //     var tunnel = new InMemoryConsumeTunnel<TConsumeData>(broker);
        //
        //     return AttachConsumer(config, part, tunnel);
        // }
        
       
    }
}