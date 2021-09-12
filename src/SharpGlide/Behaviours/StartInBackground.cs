using System.Threading.Tasks;
using SharpGlide.Context;
using SharpGlide.Context.Switch;

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