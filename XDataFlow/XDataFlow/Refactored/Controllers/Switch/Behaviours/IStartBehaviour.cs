using System.Threading.Tasks;

namespace XDataFlow.Refactored.Behaviours
{
    public interface IStartBehaviour
    {
        Task ExecuteAsync(ISwitchController switchController);
    }
}