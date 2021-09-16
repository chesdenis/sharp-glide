using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Behaviours;

namespace SharpGlide.Context.Abstractions
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