using System.Threading.Tasks;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Behaviours
{
    public class StartInBackground : Start
    {
        public override async Task ExecuteAsync(ISwitchContext switchContext)
        { 
            await Task.Run(async () =>
            {
                await base.ExecuteAsync(switchContext);
            }, switchContext.GetExecutionTokenSource().Token);
        }
    }
}