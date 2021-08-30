using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Builders;
using XDataFlow.Context;
using XDataFlow.Providers;
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
            var metaDataContext = XFlowDefault.Get<IMetaDataContext>() ?? throw new ArgumentNullException(nameof(IMetaDataContext));
            var groupContext = XFlowDefault.Get<IGroupContext>() ?? throw new ArgumentNullException(nameof(IGroupContext)); 
            var heartBeatContext = XFlowDefault.Get<IHeartBeatContext>() ?? throw new ArgumentNullException(nameof(IHeartBeatContext));
            var consumeMetrics = XFlowDefault.Get<IConsumeMetrics>() ?? throw new ArgumentNullException(nameof(IConsumeMetrics));
            
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

        public void FlowFromSelf()
        {
            var linkId = Guid.NewGuid().ToString("N");
            var topicName = $"{linkId}:{this.Name}<-[manual]";
            var queueName = $"{linkId}:{this.Name}<-[manual]";
            var routingKey = $"#";
            
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TConsumeData>(InMemoryBroker.Current);
            
            this.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, topicName, queueName, routingKey);
        }

        public void FlowTo<TTargetConsumeData, TTargetPublishData>(
            VectorPart<TTargetConsumeData, TTargetPublishData> partB)
        {
            var linkId = Guid.NewGuid().ToString("N");
            var topicName = $"{linkId}:{this.Name}->{partB.Name}";
            var queueName = $"{linkId}:{this.Name}->{partB.Name}";
            var routingKey = $"#";

            var inMemoryPublishTunnel = new InMemoryPublishTunnel<TPublishData>(InMemoryBroker.Current);
            var inMemoryConsumeTunnel = new InMemoryConsumeTunnel<TTargetConsumeData>(InMemoryBroker.Current);
            
            this.SetupPublishAsTopicToQueue(inMemoryPublishTunnel, topicName, routingKey);
            partB.SetupConsumeAsQueueFromTopic(inMemoryConsumeTunnel, topicName, queueName, routingKey);
        }
    }

    public class XFlowDefault
    {
        public static readonly Dictionary<Type, Func<object>> DefaultImpls = new Dictionary<Type, Func<object>>();

        public static void Set<T>(Func<object> builder)
        {
            DefaultImpls[typeof(T)] = builder;
        }
        
        public static T Get<T>() => (T)DefaultImpls[typeof(T)]();
    }
    
     
     
}