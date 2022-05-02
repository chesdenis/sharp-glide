using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Readers.Interfaces
{
    public interface ISingleAsyncWalker<out T>
    {
        Task WalkAsyncSingleAsync(CancellationToken cancellationToken, Func<CancellationToken,T, Task> callbackAsync);
    }

    public interface ISingleAsyncWalker<out T, in TArg>
    {
        Task WalkAsyncSingleAsync(CancellationToken cancellationToken, TArg request, Func<CancellationToken,T, Task> callbackAsync);
    }
}