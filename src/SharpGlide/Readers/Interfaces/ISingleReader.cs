using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Readers.Interfaces
{
    public interface ISingleReader<T, in TArg>
    {
        public Task<T> ReadAsync(CancellationToken cancellationToken, TArg request);
    }

    public interface ISingleReader<T>
    {
        Task<T> ReadAsync(CancellationToken cancellationToken);
    }
}