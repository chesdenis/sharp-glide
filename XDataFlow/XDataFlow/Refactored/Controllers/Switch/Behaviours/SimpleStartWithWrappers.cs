namespace XDataFlow.Refactored.Behaviours
{
    public class SimpleStartWithWrappers : IStartBehaviour
    {
        public void Execute(ISwitchController switchController)
        {
            var entryPointer = switchController.StartPointer();

            foreach (var wrapper in switchController.StartWrappers)
            {
                entryPointer = wrapper.Wrap(entryPointer);
            }

            entryPointer();
        }
    }
}