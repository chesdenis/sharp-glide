using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Context;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;
using SharpGlide.TunnelWrappers.Abstractions;

namespace SharpGlide.Parts.Abstractions
{
    public abstract class VectorPart<TConsumeData, TPublishData> : BasePart
    {
        public VectorPartContext<TConsumeData, TPublishData> VectorPartContext => 
            (VectorPartContext<TConsumeData, TPublishData>) Context;

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

        public void Publish(TPublishData data, IPublishRoute publishRoute)=>
            VectorPartContext.PublishContext.Publish(data, publishRoute);
        
        public void Publish(TPublishData data) =>
            VectorPartContext.PublishContext.Publish(data);

        public void Publish(TPublishData data, string routingKey) =>
            VectorPartContext.PublishContext.Publish(data, routingKey);

        public void Consume(TConsumeData data) => VectorPartContext.ConsumeContext.Consume(data);

        public void ConsumeRange(IEnumerable<TConsumeData> data) => VectorPartContext.ConsumeContext.ConsumeRange(data);

        public void SetupConsumeAsQueueFromTopic<TConsumeTunnel>(
            TConsumeTunnel tunnel, 
            IConsumeRoute consumeRoute) 
            where TConsumeTunnel : IConsumeTunnel<TConsumeData>
        {
            VectorPartContext.ConsumeContext.SetupBindingToTopic(tunnel, consumeRoute);
        }

        public void SetupPublishAsTopicToQueue<TPublishTunnel>(
            TPublishTunnel tunnel, 
            IPublishRoute publishRoute) 
            where TPublishTunnel : IPublishTunnel<TPublishData>
        {
            VectorPartContext.PublishContext.SetupBindingToTopic(tunnel, publishRoute);
        }

        public IEnumerable<TWrapper> GetConsumeWrapper<T, TWrapper>() where TWrapper: IConsumeWrapper<T>
        {
            foreach (var tunnelKey in 
                VectorPartContext.ConsumeContext.ConsumeTunnels.Keys)
            {
                var tunnel = VectorPartContext.ConsumeContext.ConsumeTunnels[tunnelKey];
                
                foreach (var wrapper in tunnel.OnConsumeWrappers)
                {
                    if (wrapper is TWrapper consumeWrapper)
                    {
                        yield return consumeWrapper;
                    }
                }
            }
        }
        public IEnumerable<TWrapper> GetPublishWrapper<T, TWrapper>() where TWrapper: IPublishWrapper<T>
        {
            foreach (var tunnelKey in 
                VectorPartContext.PublishContext.PublishTunnels.Keys)
            {
                var tunnel = VectorPartContext.PublishContext.PublishTunnels[tunnelKey];
                
                foreach (var wrapper in tunnel.OnPublishWrappers)
                {
                    if (wrapper is TWrapper getPublishWrapper)
                    {
                        yield return getPublishWrapper;
                    }
                }
            }
        }
    }
}