using System;
using System.Collections.Generic;
using System.Dynamic;
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

        public virtual void SetupFlow()
        {
        }
        
        public string Name { get; set; }

        protected virtual int IdleTimeoutMs => 15000;

        private DateTime lastPublish { get; set; }

        private DateTime lastConsume { get; set; }

        public bool Idle => DateTime.Now.Subtract(lastPublish).TotalMilliseconds > IdleTimeoutMs &
                            DateTime.Now.Subtract(lastConsume).TotalMilliseconds > IdleTimeoutMs;

        private readonly IDictionary<string, IPart> _children  = new Dictionary<string, IPart>();
        
        public IEnumerable<IPart> GetChildren() => this._children.Select(s => s.Value);

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
        
        public void EnumerateParts(Action<IPart> partAction)
        {
            foreach (var part in this.GetChildren())
            {
                partAction(part);
            }
        }
        
        public static Func<TFlowPart> CreateTemplate<TFlowPart>() where TFlowPart : IPart, new() => () => new TFlowPart();

        public static Func<TFlowPart> CreateTemplate<TFlowPart>(params object[] args)
            where TFlowPart : IPart
            => ()=> (TFlowPart)Activator.CreateInstance(typeof(TFlowPart), args);

        public TFlowPart AddFlowPart<TFlowPart>(TFlowPart flowPart, string name) where TFlowPart : IPart
        {
            flowPart.Name = name;
            flowPart.Status.Upsert("Name", name);

            _children.Add(name, flowPart);

            return flowPart;
        }
        
        public List<ExpandoObject> GetStatusInfo()
        {
            foreach (var partsKey in _children.Keys)
            {
                var part = _children[partsKey];
                part.CollectStatusInfo();
            }
            
            var partsStatusKeys = _children
                .Select(s => s.Value)
                .SelectMany(ss => ss.Status.Keys)
                .Distinct()
                .OrderBy(o => o)
                .ToList();

            var dataToPlot = new List<ExpandoObject>();

            foreach (var partsKey in _children.Keys)
            {
                var part = _children[partsKey];

                var partStatus = new ExpandoObject();
                var partStatusAsDict = (IDictionary<string, object>) partStatus;

                foreach (var partsStatusKey in partsStatusKeys)
                {
                    partStatusAsDict[partsStatusKey] =
                        part.Status.ContainsKey(partsStatusKey) ? part.Status[partsStatusKey] : string.Empty;
                }

                dataToPlot.Add(partStatus);
            }

            return dataToPlot;
        }

        // R
        public IList<IStartBehaviour> StartBehaviours { get; } = new List<IStartBehaviour>();
        
        // R
        public IList<IWrapper> StartWrappers { get; } = new List<IWrapper>();
        
        // R
        public IList<IStopBehaviour> StopBehaviours { get; } = new List<IStopBehaviour>();
        
        // R
        public IList<IWrapper> StopWrappers { get; } = new List<IWrapper>();
        
        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; } =
            new Dictionary<string, IPublishTunnel<TPublishData>>();
        
        // Refactored
        public void Publish(TPublishData data)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data);
                lastPublish = DateTime.Now;
            }
        }

        // Refactored
        public void Publish(TPublishData data, string routingKey)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data, routingKey);
                lastPublish = DateTime.Now;
            }
        }

        // Refactored
        public void Publish(TPublishData data, string topicName, string routingKey)
        {
            foreach (var t in this.PublishTunnels)
            {
                t.Value.Publish(data, topicName, routingKey);
                lastPublish = DateTime.Now;
            }
        }

        // Refactored
        public void ConsumeCustomData(TConsumeData data)
        {
            this.ConsumeTunnels.First().Value.Put(data);
        }

        // Refactored
        public IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; } =
            new Dictionary<string, IConsumeTunnel<TConsumeData>>();
        
        // Refactored
        public FlowPart<TConsumeData, TPublishData> ConfigureListening(IConsumeTunnel<TConsumeData> tunnel)
        {
            tunnel.RoutingKey = "#";
            tunnel.QueueName = $"InputQueueFor_{Name}";
            tunnel.TopicName = $"InputTopicFor_{Name}";
            
            return ConfigureListening(tunnel, tunnel.TopicName, tunnel.QueueName, tunnel.RoutingKey);
        }
        
        // Refactored
        public FlowPart<TConsumeData, TPublishData> ConfigureListening(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey)
        {
            tunnel.RoutingKey = routingKey;
            tunnel.QueueName = queueName;
            tunnel.TopicName = topicName;

            tunnel.SetupInfrastructure(topicName, queueName, routingKey);
            
            this.ConsumeTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);

            return this;
        }
        
        // refactored
        public FlowPart<TConsumeData, TPublishData> ConfigurePublishing(IPublishTunnel<TPublishData> tunnel, string topicName, string routingKey)
        {
            tunnel.RoutingKey = routingKey;
            tunnel.TopicName = topicName;

            tunnel.SetupInfrastructure(topicName, routingKey);
            
            this.PublishTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);
          
            return this;
        }

        // Refactored
        public TPublishWrapper CreatePublishWrapper<TPublishWrapper>(TPublishWrapper wrapper)
            where TPublishWrapper : IWrapperWithInput<TPublishData>
        {
            foreach (var tunnelKey in this.PublishTunnels.Keys)
            {
                this.PublishTunnels[tunnelKey].OnPublishWrappers.Add(wrapper);
            }

            return wrapper;
        }

        // Refactored
        public TConsumeWrapper CreateConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IWrapperWithOutput<TConsumeData>
        {
            foreach (var tunnelKey in this.ConsumeTunnels.Keys)
            {
                this.ConsumeTunnels[tunnelKey].OnConsumeWrappers.Add(wrapper);
            }
            
            return wrapper;
        }
        
        // Refactored
        public TWrapper CreateStartWrapper<TWrapper>(TWrapper wrapper)
            where TWrapper : IWrapper 
        {
            this.StartWrappers.Add(wrapper);

            return wrapper;
        }

        // Refactored
        public void CollectStatusInfo()
        {
            this.Status.Upsert("Available", 
                this.ConsumeTunnels.Select(s=>s.Value.WaitingToConsume)
                    .Sum().ToString());
            
            var estimatedTimeToFinishInSeconds = this.ConsumeTunnels.Select(s => s.Value.EstimatedTimeInSeconds)
                .Sum();
            this.Status.Upsert("ETA",
                TimeSpan.FromSeconds(estimatedTimeToFinishInSeconds).ToString("c"));
            
            var messagesPerSecond = this.ConsumeTunnels.Select(s => s.Value.MessagesPerSecond)
                .Sum();
            
            this.Status.Upsert("Speed, n/sec", messagesPerSecond.ToString());
        }
    }
}