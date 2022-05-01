using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;

namespace SharpGlide.Writers.Interfaces
{
    public interface ISingleWriter<T, in TArg>
    {
        Task Write(TArg arg,T data, IRoute route, CancellationToken cancellationToken);
        Task<T> WriteAndReturn(TArg arg, T data, IRoute route, CancellationToken cancellationToken);
    }

    public interface ISingleWriter<T>
    {
        Task Write(T data, IRoute route, CancellationToken cancellationToken);
        Task<T> WriteAndReturn(T data, IRoute route, CancellationToken cancellationToken);
    }
}