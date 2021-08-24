using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Context;
using XDataFlow.Tunnels;
using XDataFlow.Tunnels.InMemory;
using XDataFlow.Tunnels.InMemory.Messaging;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class VectorPart<TConsumeData, TPublishData> : BasePart
    {
        public CancellationTokenSource GetExecutionTokenSource() =>
            this.VectorPartContext.SwitchContext.CancellationTokenSource;
        
        public VectorPartContext<TConsumeData, TPublishData> VectorPartContext => 
            (VectorPartContext<TConsumeData, TPublishData>) this.Context;
        
        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels => 
            this.VectorPartContext.PublishContext.PublishTunnels;
        
        public abstract Task ProcessAsync(
            TConsumeData data, 
            CancellationToken cancellationToken);

        public void Publish(TPublishData data)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data);
            }
        }
        
        public void Publish(TPublishData data, string routingKey)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data, routingKey);
            }
        }
        
        public void Publish(TPublishData data, string exchange, string routingKey)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data, exchange ,routingKey);
            }
        }

        public void PushDataToFirstTunnel(TConsumeData data)
        {
            this.VectorPartContext.ConsumeContext.PushDataToFirstTunnel(data);
        }

        public void PushDataToTunnel(TConsumeData data, string tunnelKey)
        {
            this.VectorPartContext.ConsumeContext.PushDataToTunnel(data, tunnelKey);
        }

        public void SetupBindingAsTopicToQueue<TConsumeTunnel>(
            TConsumeTunnel tunnel, 
            string topicName, 
            string queueName, 
            string routingKey) 
            where TConsumeTunnel : IConsumeTunnel<TConsumeData>
        {
            this.VectorPartContext.ConsumeContext.SetupBindingToTopic(tunnel, topicName, queueName, routingKey);
        }

        public void SetupBindingAsTopicToQueue(
            string topicName, 
            string queueName, 
            string routingKey)
        {
            var tunnel = new InMemoryConsumeTunnel<TConsumeData>(InMemoryBroker.Current);
            
            this.VectorPartContext.ConsumeContext.SetupBindingToTopic(tunnel, topicName, queueName, routingKey);
        }
    }
}