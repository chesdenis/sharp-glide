using System.Threading.Tasks;
using DechFlow.Context;

namespace DechFlow.Behaviours
{
    public interface IStartBehaviour
    {
        Task ExecuteAsync(ISwitchContext switchContext);
    }
}