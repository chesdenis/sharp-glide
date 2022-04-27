using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Interfaces;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public class ReaderByRequestProxy<T, TRequest> : IReaderByRequestProxy<T, TRequest>
    {
        private readonly Func<CancellationToken, TRequest, Task<T>> _readFunc;
        private readonly Func<CancellationToken, TRequest, Task<IEnumerable<T>>> _readAllFunc;

        private readonly Func<CancellationToken, PageInfo, TRequest, Task<IEnumerable<T>>>
            _readPagedFunc;

        private readonly Func<CancellationToken, TRequest, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>> _readSpecificFunc;

        public ReaderByRequestProxy(
            Func<CancellationToken, TRequest, Task<T>> readFunc,
            Func<CancellationToken, TRequest, Task<IEnumerable<T>>> readAllFunc,
            Func<CancellationToken, PageInfo, TRequest, Task<IEnumerable<T>>> readPagedFunc,
            Func<CancellationToken, TRequest, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>> readSpecificFunc)
        {
            _readFunc = readFunc;
            _readAllFunc = readAllFunc;
            _readSpecificFunc = readSpecificFunc;
            _readPagedFunc = readPagedFunc;
        }

        public async Task<T> ReadAsync(CancellationToken cancellationToken, TRequest request)
            => await _readFunc(cancellationToken, request);

        public async Task<IEnumerable<T>> ReadAllAsync(CancellationToken cancellationToken, TRequest request)
            => await _readAllFunc(cancellationToken, request);

        public async Task<IEnumerable<T>> ReadPaged(CancellationToken cancellationToken, PageInfo pageInfo,
            TRequest request)
            => await _readPagedFunc(cancellationToken, pageInfo, request);

        public async Task<IEnumerable<T>> ReadSpecific(CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter, TRequest request)
            => await _readSpecificFunc(cancellationToken, request, filter);
    }
}