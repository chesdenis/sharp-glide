using System;
using System.Threading.Tasks;
using XDataFlow.Refactored.Controllers.Switch.Behaviours;

namespace XDataFlow.Refactored.Controllers.Switch
{
    public interface ISwitchController
    {
        Func<Task> GetStartAsyncCall();
        
        Func<Task> GetStopAsyncCall();

        IStartBehaviour StartBehaviour { get; set; }
        
        IStopBehaviour StopBehaviour { get; set; }

        Task TearUpAsync();

        Task TearDownAsync();
    }
}