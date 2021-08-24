using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Behaviours;

namespace XDataFlow.Context
{
    public abstract class SwitchContext : ISwitchContext
    {
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Func<Task> GetStartAsyncCall() => OnStartAsync;

        public Func<Task> GetStopAsyncCall() => OnStopAsync;

        public IStartBehaviour StartBehaviour { get; set; } 
       
        public IStopBehaviour StopBehaviour { get; set; }
        
        protected abstract Task OnStartAsync();
        
        protected abstract Task OnStopAsync();
        
        public async Task TearUpAsync()
        {
            CancelJobsAndReissueToken();
            await StartBehaviour.ExecuteAsync(this);
        }

        public async Task TearDownAsync()
        {
            CancelJobsAndReissueToken();
            await StopBehaviour.ExecuteAsync(this);
        }

        private void CancelJobsAndReissueToken()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();
            CancellationTokenSource = new CancellationTokenSource();
        }
    }
}