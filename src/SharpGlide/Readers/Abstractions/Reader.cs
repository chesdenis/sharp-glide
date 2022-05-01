using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers
{
    public class Reader<T> :
        ISingleReader<T>,
        ICollectionReader<T>,
        IPagedReader<T>,
        IFilteredReader<T>
    {
        private readonly Func<CancellationToken, Task<T>> _singleReadFunc;
        private readonly Func<CancellationToken, Task<IEnumerable<T>>> _collectionReadFunc;
        private readonly Func<CancellationToken, PageInfo, Task<IEnumerable<T>>> _pagedReadFunc;
        private readonly Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>> _filteredReadFunc;

        public Reader(
            Func<CancellationToken, Task<T>> singleReadFunc,
            Func<CancellationToken, Task<IEnumerable<T>>> collectionReadFunc,
            Func<CancellationToken, PageInfo, Task<IEnumerable<T>>> pagedReadFunc,
            Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>> filteredReadFunc
        )
        {
            _singleReadFunc = singleReadFunc;
            _collectionReadFunc = collectionReadFunc;
            _pagedReadFunc = pagedReadFunc;
            _filteredReadFunc = filteredReadFunc;
        }
       
        async Task<T> ISingleReader<T>.ReadAsync(CancellationToken cancellationToken)
            => await _singleReadFunc(cancellationToken);

        async Task<IEnumerable<T>> ICollectionReader<T>.ReadAsync(CancellationToken cancellationToken)
            => await _collectionReadFunc(cancellationToken);

        async  Task<IEnumerable<T>> IPagedReader<T>.ReadAsync(CancellationToken cancellationToken, PageInfo pageInfo)
            => await _pagedReadFunc(cancellationToken, pageInfo);

        async Task<IEnumerable<T>> IFilteredReader<T>.ReadAsync(CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter)
            => await _filteredReadFunc(cancellationToken, filter);
    }
    
    public class Reader<T, TRequest> :
        ISingleReader<T, TRequest>,
        ICollectionReader<T, TRequest>,
        IPagedReader<T, TRequest>,
        IFilteredReader<T, TRequest>
    {
        private readonly Func<CancellationToken, TRequest, Task<T>> _singleReadFunc;
        private readonly Func<CancellationToken, TRequest, Task<IEnumerable<T>>> _collectionReadFunc;
        private readonly Func<CancellationToken, PageInfo, TRequest, Task<IEnumerable<T>>> _pagedReadFunc;
        private readonly Func<CancellationToken, TRequest, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>> _filteredReadFunc;

        public Reader(
            Func<CancellationToken, TRequest, Task<T>> singleReadFunc,
            Func<CancellationToken, TRequest, Task<IEnumerable<T>>> collectionReadFunc,
            Func<CancellationToken, PageInfo, TRequest, Task<IEnumerable<T>>> pagedReadFunc,
            Func<CancellationToken, TRequest, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>> filteredReadFunc)
        {
            _singleReadFunc = singleReadFunc;
            _collectionReadFunc = collectionReadFunc;
            _pagedReadFunc = pagedReadFunc;
            _filteredReadFunc = filteredReadFunc;
        }


        async Task<T> ISingleReader<T, TRequest>.ReadAsync(CancellationToken cancellationToken, TRequest request) => await _singleReadFunc(cancellationToken, request);

        async Task<IEnumerable<T>> ICollectionReader<T, TRequest>.ReadAsync(CancellationToken cancellationToken, TRequest request) => await _collectionReadFunc(cancellationToken, request);

        async Task<IEnumerable<T>>  IPagedReader<T, TRequest>.ReadAsync(CancellationToken cancellationToken, PageInfo pageInfo, TRequest request) => await _pagedReadFunc(cancellationToken, pageInfo, request);

        async Task<IEnumerable<T>> IFilteredReader<T, TRequest>.ReadAsync(CancellationToken cancellationToken, TRequest request, Func<IEnumerable<T>, IEnumerable<T>> filter) => await _filteredReadFunc(cancellationToken, request, filter);
    }
}