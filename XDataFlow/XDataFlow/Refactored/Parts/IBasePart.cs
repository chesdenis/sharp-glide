namespace XDataFlow.Refactored
{
    public interface IBasePart
    {
        string Name { get; set; }

        ISwitchController SwitchController { get; }
    }
}