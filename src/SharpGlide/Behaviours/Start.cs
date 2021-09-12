using System.Threading.Tasks;
using SharpGlide.Context;
using SharpGlide.Context.Switch;

namespace SharpGlide.Behaviours
{
    public class Start : IStartBehaviour
    {
        public virtual async Task ExecuteAsync(ISwitchContext switchContext)
        {
            var invoke = switchContext.GetStartAsyncCall()?.Invoke();

            if (invoke != null) await invoke;
        }
    }
}