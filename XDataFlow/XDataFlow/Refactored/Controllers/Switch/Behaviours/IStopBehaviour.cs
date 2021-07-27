using System.Threading.Tasks;

namespace XDataFlow.Refactored.Behaviours
{
    public interface IStopBehaviour
    {
        void Execute(ISwitchController switchController);

        Task ExecuteAsync(ISwitchController switchController);
    }
}