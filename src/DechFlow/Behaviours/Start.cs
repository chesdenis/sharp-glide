using System.Threading.Tasks;
using DechFlow.Context;

namespace DechFlow.Behaviours
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