using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Readers
{
    public interface ISingleWalker<out T>
    {
        Task WalkAsync(CancellationToken cancellationToken, Action<T> callback);
    }

    public interface ISingleWalker<out T, in TArg>
    {
        Task WalkAsync(CancellationToken cancellationToken, TArg request, Action<T> callback);
    }
}