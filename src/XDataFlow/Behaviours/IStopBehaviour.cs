using System.Threading.Tasks;
using XDataFlow.Context;

namespace XDataFlow.Behaviours
{
    public interface IStopBehaviour
    {
        Task ExecuteAsync(ISwitchContext switchContext);
    }
}