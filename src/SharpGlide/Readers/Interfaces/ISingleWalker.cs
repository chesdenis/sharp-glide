using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Readers.Interfaces
{
    public interface ISingleWalker<out T>
    {
        Task WalkSingleAsync(CancellationToken cancellationToken, Action<T> callback);
    }

    public interface ISingleWalker<out T, in TArg>
    {
        Task WalkSingleAsync(CancellationToken cancellationToken, TArg request, Action<T> callback);
    }
}