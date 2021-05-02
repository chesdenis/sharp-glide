using System;
using System.Collections.Generic;
using XDataFlow.Behaviours;
using XDataFlow.Tunnels;
using XDataFlow.Wrappers;

namespace XDataFlow.Parts
{
    public interface IDataFlowPart
    {
        Action EntryPointer();
        
        IDataFlowPart Parent { get; }

        IEnumerable<IDataFlowPart> Children { get; }

        IList<IRaiseUpBehaviour> OnRaiseUp { get; }
        
        IList<IStartBehaviour> OnStarted { get; }
        IList<IStopBehaviour> OnStopped { get; }

        IList<IWrapper> OnEntryWrappers { get; }
    }

    public interface IXDataFlowPart<TIn, TOut>
    {
        Action OnStart();

        Action OnStop();

        IDictionary<string, IXDataFlowPart<TIn, TOut>> Children { get; }

        IDictionary<string, IPublishTunnel<TIn>> PublishTunnels { get; }

        IDictionary<string, IConsumeTunnel<TOut>> ConsumeTunnels { get; }

        void Consume(TOut data, string tunnelKey);

        void Publish(TIn data, string tunnelKey);
    }
}