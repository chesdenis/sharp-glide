using System.Threading.Tasks;

namespace XDataFlow.Controllers.Switch.Behaviours
{
    public interface IStopBehaviour
    {
        void Execute(ISwitchController switchController);

        Task ExecuteAsync(ISwitchController switchController);
    }
}