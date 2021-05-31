using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Behaviours;
using XDataFlow.Exceptions;
using XDataFlow.Extensions;
using XDataFlow.Tunnels;
using XDataFlow.Wrappers;

namespace XDataFlow.Parts
{
    public abstract class FlowPart<TConsumeData, TPublishData> :
        IFlowPart<TConsumeData, TPublishData>,
        IFlowPublisherPart<TPublishData>,
        IFlowConsumerPart<TConsumeData>
    {
        private CancellationTokenSource _cts;

        public string Name { get; set; }

        protected virtual int IdleTimeoutMs => 15000;

        private DateTime lastPublish { get; set; }

        private DateTime lastConsume { get; set; }

        public bool Idle => DateTime.Now.Subtract(lastPublish).TotalMilliseconds > IdleTimeoutMs &
                            DateTime.Now.Subtract(lastConsume).TotalMilliseconds > IdleTimeoutMs;

        public Dictionary<string, string> Status { get; } = new Dictionary<string, string>();

        public Action StartPointer()
        {
            return OnStart;
        }
        
        public Action StopPointer()
        {
            return OnStop;
        }
        
        private void OnStart()
        {
            _cts = new CancellationTokenSource();

            while (true)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    this.Consume<IFlowConsumerPart<TConsumeData>, TConsumeData>(ProcessMessage);
                    lastConsume = DateTime.Now;
                }
                catch (NoDataException)
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }
            }
        }

        protected abstract void ProcessMessage(TConsumeData data);

        private void OnStop()
        {
            _cts.Cancel();
        }

        public IList<IStartBehaviour> StartBehaviours { get; } = new List<IStartBehaviour>();
        public IList<IWrapper> StartWrappers { get; } = new List<IWrapper>();
        public IList<IStopBehaviour> StopBehaviours { get; } = new List<IStopBehaviour>();
        public IList<IWrapper> StopWrappers { get; } = new List<IWrapper>();
        
        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; } =
            new Dictionary<string, IPublishTunnel<TPublishData>>();
        
        public void Publish(TPublishData data)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data);
                lastPublish = DateTime.Now;
            }
        }
        
        public void Publish(TPublishData data, string routingKey)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data, routingKey);
                lastPublish = DateTime.Now;
            }
        }

        public void Publish(TPublishData data, string topicName, string routingKey)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data, topicName, routingKey);
                lastPublish = DateTime.Now;
            }
        }

        public void ConsumeCustomData(TConsumeData data)
        {
            this.ConsumeTunnels.First().Value.Put(data);
        }

        public IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; } =
            new Dictionary<string, IConsumeTunnel<TConsumeData>>();
        
        public FlowPart<TConsumeData, TPublishData> ConfigureListening(IConsumeTunnel<TConsumeData> tunnel)
        {
            tunnel.RoutingKey = "#";
            tunnel.QueueName = $"InputQueueFor_{Name}";
            tunnel.TopicName = $"InputTopicFor_{Name}";
            
            return ConfigureListening(tunnel, tunnel.TopicName, tunnel.QueueName, tunnel.RoutingKey);
        }
        
        public FlowPart<TConsumeData, TPublishData> ConfigureListening(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey)
        {
            tunnel.RoutingKey = routingKey;
            tunnel.QueueName = queueName;
            tunnel.TopicName = topicName;

            tunnel.SetupInfrastructure(topicName, queueName, routingKey);
            
            this.ConsumeTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);

            return this;
        }
        
        public FlowPart<TConsumeData, TPublishData> ConfigurePublishing(IPublishTunnel<TPublishData> tunnel, string topicName, string routingKey)
        {
            tunnel.RoutingKey = routingKey;
            tunnel.TopicName = topicName;

            tunnel.SetupInfrastructure(topicName, routingKey);
            
            this.PublishTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);
          
            return this;
        }

        public TPublishWrapper CreatePublishWrapper<TPublishWrapper>(TPublishWrapper wrapper)
            where TPublishWrapper : IWrapperWithInput<TPublishData>
        {
            foreach (var tunnelKey in this.PublishTunnels.Keys)
            {
                this.PublishTunnels[tunnelKey].OnPublishWrappers.Add(wrapper);
            }

            return wrapper;
        }

        public TConsumeWrapper CreateConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IWrapperWithOutput<TConsumeData>
        {
            foreach (var tunnelKey in this.ConsumeTunnels.Keys)
            {
                this.ConsumeTunnels[tunnelKey].OnConsumeWrappers.Add(wrapper);
            }
            
            return wrapper;
        }
        
        public TWrapper CreateStartWrapper<TWrapper>(TWrapper wrapper)
            where TWrapper : IWrapper 
        {
            this.StartWrappers.Add(wrapper);

            return wrapper;
        }

        public void CollectStatusInfo()
        {
            this.Status.Upsert("WaitingToProcess", 
                this.ConsumeTunnels.Select(s=>s.Value.WaitingToConsume)
                    .Sum().ToString());
        }
    }
}