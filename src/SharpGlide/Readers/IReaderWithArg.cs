using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers
{
    public interface IReaderWithArg<T, in TArg>
    {
        Task<T> ReadAsync(CancellationToken cancellationToken, TArg request);
        Task<IEnumerable<T>> ReadAllAsync(CancellationToken cancellationToken, TArg request);

        Task<IEnumerable<T>> ReadPaged(CancellationToken cancellationToken, PageInfo pageInfo,
            TArg request);

        Task<IEnumerable<T>> ReadSpecific(
            CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter, TArg request);
    }
}