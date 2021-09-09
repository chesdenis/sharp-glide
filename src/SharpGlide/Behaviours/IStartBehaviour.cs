using System.Threading.Tasks;
using SharpGlide.Context;

namespace SharpGlide.Behaviours
{
    public interface IStartBehaviour
    {
        Task ExecuteAsync(ISwitchContext switchContext);
    }
}