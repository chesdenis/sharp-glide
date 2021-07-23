namespace XDataFlow.Refactored.Behaviours
{
    public class SimpleStart : IStartBehaviour
    {
        public void Execute(ISwitchController switchController)
        {
            switchController.StartPointer();
        }
    }
}