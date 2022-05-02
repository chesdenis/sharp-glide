using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;

namespace SharpGlide.Writers.Interfaces
{
    public interface ISingleWriter<T, in TArg>
    {
        Task WriteSingle(TArg arg,T data, IRoute route, CancellationToken cancellationToken);
        Task<T> WriteAndReturnSingle(TArg arg, T data, IRoute route, CancellationToken cancellationToken);
    }

    public interface ISingleWriter<T>
    {
        Task WriteSingle(T data, IRoute route, CancellationToken cancellationToken);
        Task<T> WriteAndReturnSingle(T data, IRoute route, CancellationToken cancellationToken);
    }
}