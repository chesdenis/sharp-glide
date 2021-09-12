using System.Threading.Tasks;
using SharpGlide.Context;
using SharpGlide.Context.Switch;

namespace SharpGlide.Behaviours
{
    public interface IStopBehaviour
    {
        Task ExecuteAsync(ISwitchContext switchContext);
    }
}