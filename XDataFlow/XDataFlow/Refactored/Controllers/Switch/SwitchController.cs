using System;
using System.Threading.Tasks;
using XDataFlow.Refactored.Controllers.Switch.Behaviours;

namespace XDataFlow.Refactored.Controllers.Switch
{
    public abstract class SwitchController : ISwitchController
    {
        public Func<Task> GetStartAsyncCall() => OnStartAsync;

        public Func<Task> GetStopAsyncCall() => OnStopAsync;

        public IStartBehaviour StartBehaviour { get; set; } 
       
        public IStopBehaviour StopBehaviour { get; set; }
        
        protected abstract Task OnStartAsync();
        
        protected abstract Task OnStopAsync();
        
        public async Task TearUpAsync()
        {
            await StartBehaviour.ExecuteAsync(this);
        }

        public async Task TearDownAsync()
        {
            await StopBehaviour.ExecuteAsync(this);
        }
    }
}