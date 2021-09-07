using System;
using System.Threading;
using System.Threading.Tasks;
using DechFlow.Behaviours;
using DechFlow.Exceptions;

namespace DechFlow.Context
{
    public abstract class SwitchContext : ISwitchContext
    {
        private CancellationTokenSource _cts;
        public CancellationTokenSource GetExecutionTokenSource() => _cts;
        
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

            if (StopBehaviour != null)
            {
                await StopBehaviour.ExecuteAsync(this);
            }
        }

        private void ReissueToken()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                return;
            }
            
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }

        private void CancelToken()
        {
            _cts?.Cancel();
        }
    }
}