using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Context;
using XDataFlow.Registry;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts.Abstractions
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

        protected VectorPart(IDefaultRegistry defaultRegistry)
        {
            var metaDataContext = defaultRegistry.Get<IMetaDataContext>() ?? throw new ArgumentNullException(nameof(IMetaDataContext));
            var groupContext = defaultRegistry.Get<IGroupContext>() ?? throw new ArgumentNullException(nameof(IGroupContext)); 
            var heartBeatContext = defaultRegistry.Get<IHeartBeatContext>() ?? throw new ArgumentNullException(nameof(IHeartBeatContext));
            var consumeMetrics = defaultRegistry.Get<IConsumeMetrics>() ?? throw new ArgumentNullException(nameof(IConsumeMetrics));

            var settingsContext = defaultRegistry.Get<ISettingsContext>() ??
                                  throw new ArgumentNullException(nameof(ISettingsContext));
            
            var consumeContext = new ConsumeContext<TConsumeData>();
            var publishContext = new PublishContext<TPublishData>(heartBeatContext);
            
            var switchContext = new VectorPartSwitchContext<TConsumeData, TPublishData>(
                this, 
                consumeContext);
            
            Context = new VectorPartContext<TConsumeData, TPublishData>
                (metaDataContext,
                    groupContext,
                    heartBeatContext,
                    consumeMetrics,
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

        public void ConsumeData(TConsumeData data)
        {
            VectorPartContext.ConsumeContext.ConsumeData(data);
        }

        public void ConsumeData(TConsumeData data, string tunnelKey)
        {
            VectorPartContext.ConsumeContext.ConsumeData(data, tunnelKey);
        }

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