using System.Threading.Tasks;
using SharpGlide.Context;

namespace SharpGlide.Behaviours
{
    public interface IStopBehaviour
    {
        Task ExecuteAsync(ISwitchContext switchContext);
    }
}