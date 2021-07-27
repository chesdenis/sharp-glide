using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XDataFlow.Refactored.Behaviours;
using XDataFlow.Wrappers;

namespace XDataFlow.Refactored
{
    public interface ISwitchController
    {
        Func<Task> GetStartAsyncCall();
        
        Func<Task> GetStopAsyncCall();

        IStartBehaviour StartBehaviour { get; set; }
        
        IStopBehaviour StopBehaviour { get; set; }

        Task StartAsync();

        Task StopAsync();
    }
}