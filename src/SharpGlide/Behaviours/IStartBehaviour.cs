using System.Threading.Tasks;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Behaviours
{
    public interface IStartBehaviour
    {
        Task ExecuteAsync(ISwitchContext switchContext);
    }
}