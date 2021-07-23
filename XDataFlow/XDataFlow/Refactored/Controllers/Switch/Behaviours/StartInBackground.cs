using System.Threading.Tasks;

namespace XDataFlow.Refactored.Behaviours
{
    public class StartInBackground : IStartBehaviour
    {
        public void Execute(ISwitchController switchController)
        {
            Task.Run(() => switchController.StartPointer()());
        }
    }
}