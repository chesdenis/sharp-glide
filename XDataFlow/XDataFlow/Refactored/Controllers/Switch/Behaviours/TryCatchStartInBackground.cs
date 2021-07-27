using System;
using System.Threading;
using System.Threading.Tasks;

namespace XDataFlow.Refactored.Behaviours
{
    public class TryCatchStartInBackground : TryCatchStart
    {
        private readonly CancellationToken _ct;

        public TryCatchStartInBackground(
            Action onStart, 
            Action<Exception> onException, 
            Action onFinish,
            CancellationToken ct) 
            : base(onStart, onException, onFinish)
        {
            _ct = ct;
        }

        public override async Task ExecuteAsync(ISwitchController switchController)
        {
            await Task.Run(async () =>
            {
                await base.ExecuteAsync(switchController);
            }, _ct);
        }
    }
}