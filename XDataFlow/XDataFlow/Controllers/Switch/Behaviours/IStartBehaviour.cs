using System.Threading.Tasks;

namespace XDataFlow.Controllers.Switch.Behaviours
{
    public interface IStartBehaviour
    {
        Task ExecuteAsync(ISwitchController switchController);
    }
}