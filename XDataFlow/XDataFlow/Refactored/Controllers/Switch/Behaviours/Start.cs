using System.Threading.Tasks;

namespace XDataFlow.Refactored.Controllers.Switch.Behaviours
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