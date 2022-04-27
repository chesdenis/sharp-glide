using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public interface IReaderByRequestProxy<T, in TRequest>
    {
        Task<T> ReadAsync(CancellationToken cancellationToken, TRequest request);
        Task<IEnumerable<T>> ReadAllAsync(CancellationToken cancellationToken, TRequest request);

        Task<IEnumerable<T>> ReadPaged(CancellationToken cancellationToken, PageInfo pageInfo,
            TRequest request);

        Task<IEnumerable<T>> ReadSpecific(
            CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter, TRequest request);
    }
}