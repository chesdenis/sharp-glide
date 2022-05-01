using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Readers
{
    public interface ICollectionReader<T, in TArg>
    {
        Task<IEnumerable<T>> ReadAsync(CancellationToken cancellationToken, TArg request);
    }

    public interface ICollectionReader<T>
    {
        Task<IEnumerable<T>> ReadAsync(CancellationToken cancellationToken);
    }
}