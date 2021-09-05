using System.Threading.Tasks;
using XDataFlow.Context;

namespace XDataFlow.Behaviours
{
    public interface IStopBehaviour
    {
        void Execute(ISwitchContext switchContext);

        Task ExecuteAsync(ISwitchContext switchContext);
    }
}