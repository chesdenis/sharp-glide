using System;
using System.Threading.Tasks;

namespace XDataFlow.Refactored.Behaviours
{
    public class Start : IStartBehaviour
    {
        public virtual async Task ExecuteAsync(ISwitchController switchController)
        {
            var invoke = switchController.GetStartAsyncCall()?.Invoke();

            if (invoke != null) await invoke;
        }
    }
}