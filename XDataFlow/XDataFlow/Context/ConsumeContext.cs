using System;
using System.Collections.Generic;
using System.Linq;
using XDataFlow.Tunnels;
using XDataFlow.Wrappers;

namespace XDataFlow.Context
{
    public class ConsumeContext<TConsumeData> : IConsumeContext<TConsumeData>
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
        
        public void SetupBindingToTopic(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey)
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
            
            SetupBindingToTopic(tunnel, tunnel.TopicName, tunnel.QueueName, tunnel.RoutingKey);
        }
        
        public void PushDataToFirstTunnel(TConsumeData data)
        {
            this.ConsumeTunnels.First().Value.Put(data);
        }

        public void PushDataToTunnel(TConsumeData data, string tunnelKey)
        {
            this.ConsumeTunnels.First(f=>f.Key == tunnelKey).Value.Put(data);
        }

        // TODO: make it async
        public IEnumerable<TConsumeData> ReadAndConsumeData()
        {
            var tunnels = this.ConsumeTunnels;

            foreach (var tunnelKey in tunnels.Keys)
            {
                // TODO: make it awaitable
                var consumeData = tunnels[tunnelKey].Consume();
                yield return consumeData;
            }
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