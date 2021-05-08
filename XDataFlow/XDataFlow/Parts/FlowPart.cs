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

        public void Publish(TPublishData data, Func<KeyValuePair<string, IPublishTunnel<TPublishData>>, bool> predicate)
        {
            var tunnels = this.PublishTunnels.Where(predicate).ToList();

            foreach (var t in tunnels)
            {
                t.Value.Publish(data);
            }
        }

        public void Publish(TPublishData data)
        {
            this.Publish(data, w => true);
        }

        public IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; } =
            new Dictionary<string, IConsumeTunnel<TConsumeData>>();
    }
}