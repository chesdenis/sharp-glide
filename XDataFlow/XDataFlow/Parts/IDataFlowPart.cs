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
}