using System;
using System.Collections.Generic;
using System.Linq;
using XDataFlow.Tunnels;
using XDataFlow.Wrappers;

namespace XDataFlow.Parts.Interfaces
{
    public class ConsumeController<TConsumeData> : IConsumeController<TConsumeData>
    {
        public IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; } =
            new Dictionary<string, IConsumeTunnel<TConsumeData>>();
        
        public TConsumeWrapper AddConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IWrapperWithOutput<TConsumeData>
        {
            foreach (var tunnelKey in this.ConsumeTunnels.Keys)
            {
                this.ConsumeTunnels[tunnelKey].OnConsumeWrappers.Add(wrapper);
            }
            
            return wrapper;
        }
        
        public void ConfigureListening(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey)
        {
            tunnel.RoutingKey = routingKey;
            tunnel.QueueName = queueName;
            tunnel.TopicName = topicName;

            tunnel.SetupInfrastructure(topicName, queueName, routingKey);
            
            this.ConsumeTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);
        }
        
        public void ConfigureListening(IConsumeTunnel<TConsumeData> tunnel, string name)
        {
            tunnel.RoutingKey = "#";
            tunnel.QueueName = $"InputQueueFor_{name}";
            tunnel.TopicName = $"InputTopicFor_{name}";
            
            ConfigureListening(tunnel, tunnel.TopicName, tunnel.QueueName, tunnel.RoutingKey);
        }
        
        public void ConsumeCustomData(TConsumeData data)
        {
            this.ConsumeTunnels.First().Value.Put(data);
        }

        public int GetWaitingToConsumeAmount() =>
            this.ConsumeTunnels
                .Select(s => s.Value.WaitingToConsume)
                .Sum();

        public TimeSpan GetEstimatedTime() =>
            TimeSpan.FromSeconds(
                this.ConsumeTunnels
                    .Select(s => s.Value.EstimatedTimeInSeconds)
                    .Sum());

        public int GetMessagesPerSecond() =>
            this.ConsumeTunnels
                .Select(s => s.Value.MessagesPerSecond)
                .Sum();
    }
}