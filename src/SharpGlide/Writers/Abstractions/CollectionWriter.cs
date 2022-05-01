using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Writers.Interfaces;

namespace SharpGlide.Writers.Abstractions
{
    public class CollectionWriter<T, TArg> : ICollectionWriter<T, TArg>
    {
        private readonly Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task> _collectionWriteFunc;
        private readonly Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>> _collectionWriteAndReturnFunc;

        public CollectionWriter(
            Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task> collectionWriteFunc,
            Func<TArg, IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>> collectionWriteAndReturnFunc
        )
        {
            _collectionWriteFunc = collectionWriteFunc;
            _collectionWriteAndReturnFunc = collectionWriteAndReturnFunc;
        }
        
        async Task ICollectionWriter<T,TArg>.Write(TArg arg, IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken)
            => await _collectionWriteFunc(arg, dataRange, route, cancellationToken);

        async Task<IEnumerable<T>> ICollectionWriter<T,TArg>.WriteAndReturn(TArg arg, IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken) 
            => await _collectionWriteAndReturnFunc(arg, dataRange, route, cancellationToken);
    }
    
    public class CollectionWriter<T> : ICollectionWriter<T>
    {
        private readonly Func<IEnumerable<T>, IRoute, CancellationToken, Task> _collectionWriteFunc;
        private readonly Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>> _collectionWriteAndReturnFunc;

        public CollectionWriter(
            Func<IEnumerable<T>, IRoute, CancellationToken, Task> collectionWriteFunc,
            Func<IEnumerable<T>, IRoute, CancellationToken, Task<IEnumerable<T>>> collectionWriteAndReturnFunc
        )
        {
            _collectionWriteFunc = collectionWriteFunc;
            _collectionWriteAndReturnFunc = collectionWriteAndReturnFunc;
        }
        
        async Task ICollectionWriter<T>.Write(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken) => await _collectionWriteFunc(dataRange, route, cancellationToken);

        async Task<IEnumerable<T>> ICollectionWriter<T>.WriteAndReturn(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken) => await _collectionWriteAndReturnFunc(dataRange, route, cancellationToken);
    }
}