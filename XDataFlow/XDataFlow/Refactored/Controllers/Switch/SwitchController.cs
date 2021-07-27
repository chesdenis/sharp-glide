using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XDataFlow.Refactored.Behaviours;
using XDataFlow.Wrappers;

namespace XDataFlow.Refactored
{
    public abstract class SwitchController : ISwitchController
    {
        public Func<Task> GetStartAsyncCall() => OnStartAsync;

        public Func<Task> GetStopAsyncCall() => OnStopAsync;

        public IStartBehaviour StartBehaviour { get; set; } 
       
        public IStopBehaviour StopBehaviour { get; set; }
        
        protected abstract Task OnStartAsync();
        
        protected abstract Task OnStopAsync();
        
        public async Task StartAsync()
        {
            await StartBehaviour.ExecuteAsync(this);
        }

        public async Task StopAsync()
        {
            await StopBehaviour.ExecuteAsync(this);
        }
    }
}