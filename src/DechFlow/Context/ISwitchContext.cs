using System;
using System.Threading;
using System.Threading.Tasks;
using DechFlow.Behaviours;

namespace DechFlow.Context
{
    public interface ISwitchContext
    {
        CancellationTokenSource GetExecutionTokenSource();

        Func<Task> GetStartAsyncCall();
        
        Func<Task> GetStopAsyncCall();

        IStartBehaviour StartBehaviour { get; set; }
        
        IStopBehaviour StopBehaviour { get; set; }

        Task TearUpAsync();

        Task TearDownAsync();
    }
}