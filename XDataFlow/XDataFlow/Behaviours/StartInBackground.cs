using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Context;

namespace XDataFlow.Behaviours
{
    public class StartInBackground : Start
    {
        public override async Task ExecuteAsync(ISwitchContext switchContext)
        { 
            await Task.Run(async () =>
            {
                await base.ExecuteAsync(switchContext);
            }, switchContext.CancellationTokenSource.Token);
        }
    }
}