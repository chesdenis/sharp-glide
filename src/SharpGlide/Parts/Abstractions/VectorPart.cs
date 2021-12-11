using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Context;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Parts.Abstractions
{
    public abstract class VectorPart<TConsumeData, TPublishData> : BasePart
    {
        public VectorPartContext<TConsumeData, TPublishData> VectorPartContext => 
            (VectorPartContext<TConsumeData, TPublishData>) Context;
        
        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels => 
            VectorPartContext.PublishContext.PublishTunnels;
        
        public abstract Task ProcessAsync(
            TConsumeData data, 
            CancellationToken cancellationToken);

        protected VectorPart()
        {
            var metaDataContext = new MetaDataContext();
            var groupContext = new GroupContext();

            var settingsContext = new SettingsContext();
            
            var consumeContext = new ConsumeContext<TConsumeData>();
            var heartBeatContext =
                new VectorHeartBeatContext(groupContext, metaDataContext);
            
            var publishContext = new PublishContext<TPublishData>(heartBeatContext);
            
            var switchContext = new VectorPartSwitchContext<TConsumeData, TPublishData>(
                this, 
                consumeContext);
            
            Context = new VectorPartContext<TConsumeData, TPublishData>
                (metaDataContext,
                    groupContext,
                    heartBeatContext,
                    switchContext,
                    settingsContext,
                    consumeContext,
                    publishContext
                    );
        }

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

        public void Push(TConsumeData data) => VectorPartContext.ConsumeContext.Push(data);

        public void PushRange(IEnumerable<TConsumeData> data) => VectorPartContext.ConsumeContext.PushRange(data);

        public void SetupConsumeAsQueueFromTopic<TConsumeTunnel>(
            TConsumeTunnel tunnel, 
            string topicName, 
            string queueName, 
            string routingKey) 
            where TConsumeTunnel : IConsumeTunnel<TConsumeData>
        {
            VectorPartContext.ConsumeContext.SetupBindingToTopic(tunnel, topicName, queueName, routingKey);
        }

        public void SetupPublishAsTopicToQueue<TPublishTunnel>(
            TPublishTunnel tunnel, 
            string topicName, 
            string routingKey) 
            where TPublishTunnel : IPublishTunnel<TPublishData>
        {
            VectorPartContext.PublishContext.SetupBindingToTopic(tunnel, topicName, routingKey);
        }
    }
}