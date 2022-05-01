using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers.Interfaces
{
    public interface IPagedReader<T, in TArg>
    {
        Task<IEnumerable<T>> ReadAsync(CancellationToken cancellationToken, PageInfo pageInfo,
            TArg request);
    }

    public interface IPagedReader<T>
    {
        Task<IEnumerable<T>> ReadAsync(CancellationToken cancellationToken, PageInfo pageInfo);
    }
}