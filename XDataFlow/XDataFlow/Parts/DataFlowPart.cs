using System;
using System.Collections.Generic;
using XDataFlow.Behaviours;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public abstract class DataFlowPart : IDataFlowPart
    {
        public Action EntryPointer()
        {
            return OnEntry;
        }
        
        protected abstract void OnEntry();

        public IDataFlowPart Parent { get; }
        
        public IEnumerable<IDataFlowPart> Children { get; } = new List<IDataFlowPart>();
        public IList<IRaiseUpBehaviour> OnRaiseUp { get; } = new List<IRaiseUpBehaviour>();

        public IList<IPublishTunnel> PublishTunnels { get; } = new List<IPublishTunnel>();
        public IList<IConsumeTunnel> ConsumeTunnels { get; } = new List<IConsumeTunnel>();
        
        public IList<IStartBehaviour> OnStarted { get; } = new List<IStartBehaviour>();
        public IList<IStopBehaviour> OnStopped { get; } = new List<IStopBehaviour>();
    }
}