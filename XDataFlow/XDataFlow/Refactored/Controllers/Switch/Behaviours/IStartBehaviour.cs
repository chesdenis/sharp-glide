using System.Threading.Tasks;

namespace XDataFlow.Refactored.Controllers.Switch.Behaviours
{
    public interface IStartBehaviour
    {
        Task ExecuteAsync(ISwitchController switchController);
    }
}