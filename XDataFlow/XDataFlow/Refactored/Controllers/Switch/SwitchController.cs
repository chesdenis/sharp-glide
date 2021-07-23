using System;
using System.Collections.Generic;
using XDataFlow.Refactored.Behaviours;
using XDataFlow.Wrappers;

namespace XDataFlow.Refactored
{
    public abstract class SwitchController : ISwitchController
    {
        public Action StartPointer() => OnStart;

        public Action StopPointer() => OnStop;
        
        public IList<IStartBehaviour> StartBehaviours { get; } = new List<IStartBehaviour>();
        public IList<IWrapper> StartWrappers { get; } = new List<IWrapper>();
        public IList<IStopBehaviour> StopBehaviours { get; } = new List<IStopBehaviour>();
        public IList<IWrapper> StopWrappers { get; } = new List<IWrapper>();
        
        public TWrapper AddStartWrapper<TWrapper>(TWrapper wrapper)
            where TWrapper : IWrapper 
        {
            this.StartWrappers.Add(wrapper);

            return wrapper;
        }
        
        public abstract void OnStart();

        public abstract void OnStop();

        public void Start()
        {
            foreach (var behaviour in StartBehaviours) behaviour.Execute(this);
        }
    }
}