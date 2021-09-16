using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Behaviours;
using SharpGlide.Context.Abstractions;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Context
{
    public abstract class SwitchContext : ISwitchContext
    {
        private readonly IBasePart _part;
        private CancellationTokenSource _cts;
        public CancellationTokenSource GetExecutionTokenSource() => _cts;

        protected SwitchContext(IBasePart part)
        {
            _part = part;
        }
        
        public Func<Task> GetStartAsyncCall() => OnStartAsync;

        public Func<Task> GetStopAsyncCall() => OnStopAsync;

        public IStartBehaviour StartBehaviour { get; set; } 
       
        public IStopBehaviour StopBehaviour { get; set; }
        
        protected abstract Task OnStartAsync();
        
        protected abstract Task OnStopAsync();
        
        public async Task TearUpAsync()
        {
            ReissueToken();

            StartBehaviour ??= new TryCatchStartInBackground(() =>
            {
                // TODO: implement start timestamps here + reporting
            }, () =>
            {
                
            },
                ex =>
                {
                    _part.ReportException(ex);
                },
                () =>
                {
                    
                });
            
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