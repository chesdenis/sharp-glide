using System;
using System.Collections.Generic;
using XDataFlow.Refactored.Behaviours;
using XDataFlow.Wrappers;

namespace XDataFlow.Refactored
{
    public interface ISwitchController
    {
        Action StartPointer();
        
        Action StopPointer();

        void OnStart();

        void OnStop();
        IList<IStartBehaviour> StartBehaviours { get; }
        IList<IWrapper> StartWrappers { get; }
        IList<IStopBehaviour> StopBehaviours { get; }
        IList<IWrapper> StopWrappers { get; }

        TWrapper AddStartWrapper<TWrapper>(TWrapper wrapper)
            where TWrapper : IWrapper;
    }
}