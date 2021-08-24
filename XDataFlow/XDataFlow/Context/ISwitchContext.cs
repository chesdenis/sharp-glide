using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Behaviours;

namespace XDataFlow.Context
{
    public interface ISwitchContext
    {
        CancellationTokenSource CancellationTokenSource { get; set; }

        Func<Task> GetStartAsyncCall();
        
        Func<Task> GetStopAsyncCall();

        IStartBehaviour StartBehaviour { get; set; }
        
        IStopBehaviour StopBehaviour { get; set; }

        Task TearUpAsync();

        Task TearDownAsync();
    }
}