using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Behaviours;
using XDataFlow.Exceptions;

namespace XDataFlow.Context
{
    public abstract class SwitchContext : ISwitchContext
    {
        private CancellationTokenSource _cancellationTokenSource;
        public CancellationTokenSource GetExecutionTokenSource() => 
            _cancellationTokenSource;
        
        public Func<Task> GetStartAsyncCall() => OnStartAsync;

        public Func<Task> GetStopAsyncCall() => OnStopAsync;

        public IStartBehaviour StartBehaviour { get; set; } 
       
        public IStopBehaviour StopBehaviour { get; set; }
        
        protected abstract Task OnStartAsync();
        
        protected abstract Task OnStopAsync();
        
        public async Task TearUpAsync()
        {
            ReissueToken();

            if (StartBehaviour == null)
            {
                throw new StartBehaviourWasNotConfiguredException();
            }
            
            await StartBehaviour.ExecuteAsync(this);
        }

        public async Task TearDownAsync()
        {
            CancelToken();

            if (StopBehaviour == null)
            {
                throw new StopBehaviourWasNotConfiguredException();
            }
            
            await StopBehaviour.ExecuteAsync(this);
        }

        private void ReissueToken()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }
            
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void CancelToken()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}