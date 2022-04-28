using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers
{
    public interface IReader<T>
    {
        Task<T> ReadAsync(CancellationToken cancellationToken);
        Task<IEnumerable<T>> ReadAllAsync(CancellationToken cancellationToken);

        Task<IEnumerable<T>> ReadPaged(CancellationToken cancellationToken,
            PageInfo pageInfo);
        Task<IEnumerable<T>> ReadSpecific(CancellationToken cancellationToken, Func<IEnumerable<T>, IEnumerable<T>> filter);
    }
}