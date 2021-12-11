using System;
using System.Threading.Tasks;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Behaviours
{
    public class TryCatchStartInBackground : TryCatchStart
    {
        public TryCatchStartInBackground(
            Action onStart, 
            Action onProcessedOk, 
            Action<Exception> onProcessedWithFailure, 
            Action onFinalise) 
            : base(
                onStart, 
                onProcessedOk, 
                onProcessedWithFailure, 
                onFinalise)
        {
        }

        public override async Task ExecuteAsync(ISwitchContext switchContext)
        {
            await Task.Run(async () =>
            {
                await base.ExecuteAsync(switchContext);
            }, switchContext.GetExecutionTokenSource().Token);
        }
    }
}