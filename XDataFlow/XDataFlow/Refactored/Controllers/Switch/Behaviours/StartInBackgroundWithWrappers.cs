using System.Threading.Tasks;

namespace XDataFlow.Refactored.Behaviours
{
    public class StartInBackgroundWithWrappers : IStartBehaviour
    {
        public void Execute(ISwitchController switchController)
        {
            var entryPointer = switchController.StartPointer();

            foreach (var wrapper in switchController.StartWrappers)
            {
                entryPointer = wrapper.Wrap(entryPointer);
            }

            Task.Run(() => entryPointer());
        }
    }
}