using System;
using System.Threading.Tasks;
using XDataFlow.Controllers.Switch.Behaviours;

namespace XDataFlow.Controllers.Switch
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