using System.Threading;
using System.Threading.Tasks;

namespace XDataFlow.Controllers.Switch.Behaviours
{
    public class StartInBackground : Start
    {
        private readonly CancellationToken _ct;
        
        public StartInBackground(CancellationToken ct)
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