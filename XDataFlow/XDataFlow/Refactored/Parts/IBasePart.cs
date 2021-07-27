using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
{
    public interface IBasePart
    {
        string Name { get; set; }

        ISwitchController SwitchController { get; }
    }
}