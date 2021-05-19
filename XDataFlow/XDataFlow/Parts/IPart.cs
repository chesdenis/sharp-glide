using System;
using System.Collections.Generic;
using XDataFlow.Behaviours;
using XDataFlow.Wrappers;

namespace XDataFlow.Parts
{
    public interface IPart
    {
        string Name { get; set; }

        Dictionary<string,string> Status { get; }

        void CollectStatusInfo();

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