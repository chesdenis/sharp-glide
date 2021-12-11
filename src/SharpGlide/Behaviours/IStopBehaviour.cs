using System.Threading.Tasks;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Behaviours
{
    public interface IStopBehaviour
    {
        Task ExecuteAsync(ISwitchContext switchContext);
    }
}