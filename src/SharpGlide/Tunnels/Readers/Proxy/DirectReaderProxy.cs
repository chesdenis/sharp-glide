using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public class DirectReaderProxy<T> : IDirectReaderProxy<T>
    {
        private readonly Func<CancellationToken, Task<T>> _readFunc;
        private readonly Func<CancellationToken, Task<IEnumerable<T>>> _readAllFunc;
        private readonly Func<CancellationToken, PageInfo, Task<IEnumerable<T>>> _readPagedFunc;
        private readonly Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>> _readSpecificFunc;

        public DirectReaderProxy(
            Func<CancellationToken, Task<T>> readFunc,
            Func<CancellationToken, Task<IEnumerable<T>>> readAllFunc,
            Func<CancellationToken, PageInfo, Task<IEnumerable<T>>> readPagedFunc,
            Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>> readSpecificFunc
        )
        {
            _readFunc = readFunc;
            _readAllFunc = readAllFunc;
            _readPagedFunc = readPagedFunc;
            _readSpecificFunc = readSpecificFunc;
        }

        public async Task<T> ReadAsync(CancellationToken cancellationToken) => await _readFunc(cancellationToken);

        public async Task<IEnumerable<T>> ReadAllAsync(CancellationToken cancellationToken) =>
            await _readAllFunc(cancellationToken);

        public async Task<IEnumerable<T>> ReadPaged(CancellationToken cancellationToken,
            PageInfo pageInfo)
            => await _readPagedFunc(cancellationToken, pageInfo);

        public async Task<IEnumerable<T>> ReadSpecific(
            CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter) => await _readSpecificFunc(cancellationToken, filter);
    }
}