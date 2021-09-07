using System.Threading.Tasks;
using DechFlow.Context;

namespace DechFlow.Behaviours
{
    public interface IStopBehaviour
    {
        Task ExecuteAsync(ISwitchContext switchContext);
    }
}