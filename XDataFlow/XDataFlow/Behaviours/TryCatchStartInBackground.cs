using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Context;

namespace XDataFlow.Behaviours
{
    public class TryCatchStartInBackground : TryCatchStart
    {
        private readonly CancellationToken _ct;
        
        public TryCatchStartInBackground(
            Action onStart, 
            Action onProcessedOk, 
            Action<Exception> onProcessedWithFailure, 
            Action onFinalise, 
            CancellationToken ct) 
            : base(
                onStart, 
                onProcessedOk, 
                onProcessedWithFailure, 
                onFinalise)
        {
            _ct = ct;
        }

        public override async Task ExecuteAsync(ISwitchContext switchContext)
        {
            await Task.Run(async () =>
            {
                await base.ExecuteAsync(switchContext);
            }, _ct);
        }
    }
}