using System;
using System.Collections.Generic;
using System.Dynamic;
using XDataFlow.Behaviours;
using XDataFlow.Wrappers;

namespace XDataFlow.Parts
{
    public interface IPart
    {
        string Name { get; set; }

        Dictionary<string,string> Status { get; }
        
        bool Idle { get; }

        IEnumerable<IPart> GetChildren();

        void EnumerateParts(Action<IPart> partAction);
        
        TFlowPart AddFlowPart<TFlowPart>(TFlowPart flowPart, string name) where TFlowPart : IPart;

        void CollectStatusInfo();

        List<ExpandoObject> GetStatusInfo();

        Action StartPointer();

        Action StopPointer();
        
        IList<IStartBehaviour> StartBehaviours { get; }
        
        IList<IWrapper> StartWrappers { get; }
        
        IList<IStopBehaviour> StopBehaviours { get; }
        
        IList<IWrapper> StopWrappers { get; }
        
        TWrapper CreateStartWrapper<TWrapper>(TWrapper wrapper)
            where TWrapper : IWrapper;
    }
}