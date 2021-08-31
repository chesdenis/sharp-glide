using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Builders;
using XDataFlow.Context;
using XDataFlow.Providers;
using XDataFlow.Registry;
using XDataFlow.Tunnels;
using XDataFlow.Tunnels.InMemory;
using XDataFlow.Tunnels.InMemory.Messaging;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class VectorPart<TConsumeData, TPublishData> : BasePart
    {
        public VectorPartContext<TConsumeData, TPublishData> VectorPartContext => 
            (VectorPartContext<TConsumeData, TPublishData>) this.Context;
        
        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels => 
            this.VectorPartContext.PublishContext.PublishTunnels;
        
        public abstract Task ProcessAsync(
            TConsumeData data, 
            CancellationToken cancellationToken);

        protected VectorPart()
        {
            var metaDataContext = XFlowDefaultRegistry.Get<IMetaDataContext>() ?? throw new ArgumentNullException(nameof(IMetaDataContext));
            var groupContext = XFlowDefaultRegistry.Get<IGroupContext>() ?? throw new ArgumentNullException(nameof(IGroupContext)); 
            var heartBeatContext = XFlowDefaultRegistry.Get<IHeartBeatContext>() ?? throw new ArgumentNullException(nameof(IHeartBeatContext));
            var consumeMetrics = XFlowDefaultRegistry.Get<IConsumeMetrics>() ?? throw new ArgumentNullException(nameof(IConsumeMetrics));
            
            var consumeContext = new ConsumeContext<TConsumeData>();
            var publishContext = new PublishContext<TConsumeData,TPublishData>(heartBeatContext);
            
            var switchContext = new VectorPartSwitchContext<TConsumeData, TPublishData>(
                this, 
                consumeContext);
            
            this.Context = new VectorPartContext<TConsumeData, TPublishData>
                (metaDataContext,
                    groupContext,
                    heartBeatContext,
                    consumeMetrics,
                    switchContext,
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
            this.VectorPartContext.ConsumeContext.ConsumeData(data);
        }

        public void ConsumeData(TConsumeData data, string tunnelKey)
        {
            this.VectorPartContext.ConsumeContext.ConsumeData(data, tunnelKey);
        }

        public void SetupConsumeAsQueueFromTopic<TConsumeTunnel>(
            TConsumeTunnel tunnel, 
            string topicName, 
            string queueName, 
            string routingKey) 
            where TConsumeTunnel : IConsumeTunnel<TConsumeData>
        {
            this.VectorPartContext.ConsumeContext.SetupBindingToTopic(tunnel, topicName, queueName, routingKey);
        }

        public void SetupPublishAsTopicToQueue<TPublishTunnel>(
            TPublishTunnel tunnel, 
            string topicName, 
            string routingKey) 
            where TPublishTunnel : IPublishTunnel<TPublishData>
        {
            this.VectorPartContext.PublishContext.SetupBindingToTopic(tunnel, topicName, routingKey);
        }

        public static VectorPart<TConsumeData, TPublishData> CreateUsing(IPartBuilder partBuilder = null)
        {
            if (partBuilder == null)
            {
                throw new NotImplementedException("Create default part builder as singleton");
            }
            
            return partBuilder.CreateVectorPart<VectorPart<TConsumeData, TPublishData>, TConsumeData, TPublishData>();
        }

        public static VectorPart<TConsumeData, TPublishData> CreateUsing(
            Func<VectorPart<TConsumeData, TPublishData>> partFunc, IPartBuilder partBuilder = null)
        {
            if (partBuilder == null)
            {
                throw new NotImplementedException("Create default part builder as singleton");
            }
            
            return partBuilder.CreateVectorPart<VectorPart<TConsumeData, TPublishData>, TConsumeData, TPublishData>(partFunc);
        }
    }
}