using System.Threading.Tasks;

namespace XDataFlow.Refactored.Controllers.Switch.Behaviours
{
    public interface IStopBehaviour
    {
        void Execute(ISwitchController switchController);

        Task ExecuteAsync(ISwitchController switchController);
    }
}