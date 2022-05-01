using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Readers.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers.Abstractions
{
    public class Walker<T> :
        ISingleWalker<T>,
        IPagedWalker<T>,
        ISingleAsyncWalker<T>,
        IPagedAsyncWalker<T>
    {
        private readonly Func<CancellationToken, Action<T>, Task> _singleWalkFunc;
        private readonly Func<CancellationToken, Func<CancellationToken, T, Task>, Task> _singleAsyncWalkFunc;
        private readonly Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task> _pagedWalkFunc;
        private readonly Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task> _pagedAsyncWalkFunc;

        public Walker(
            Func<CancellationToken, Action<T>, Task> singleWalkFunc,
            Func<CancellationToken, Func<CancellationToken, T, Task>, Task> singleAsyncWalkFunc ,
            Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task> pagedWalkFunc,
            Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task> pagedAsyncWalkFunc)
        {
            _singleWalkFunc = singleWalkFunc;
            _singleAsyncWalkFunc = singleAsyncWalkFunc;
            _pagedWalkFunc = pagedWalkFunc;
            _pagedAsyncWalkFunc = pagedAsyncWalkFunc;
        }
        
        async Task ISingleWalker<T>.WalkAsync(CancellationToken cancellationToken, Action<T> callback) => await _singleWalkFunc(cancellationToken, callback);

        async Task IPagedWalker<T>.WalkAsync(CancellationToken cancellationToken, PageInfo pageInfo, Action<IEnumerable<T>> callback) => await _pagedWalkFunc(cancellationToken, pageInfo, callback);

        async Task ISingleAsyncWalker<T>.WalkAsync(CancellationToken cancellationToken, Func<CancellationToken, T, Task> callbackAsync) => await _singleAsyncWalkFunc(cancellationToken, callbackAsync);

        async Task IPagedAsyncWalker<T>.WalkAsync(CancellationToken cancellationToken, PageInfo pageInfo, Func<CancellationToken, IEnumerable<T>, Task> callbackAsync) => await _pagedAsyncWalkFunc(cancellationToken, pageInfo, callbackAsync);
    }
    
    public class Walker<T, TArg> :  
        ISingleWalker<T, TArg>,
        IPagedWalker<T, TArg>,
        ISingleAsyncWalker<T, TArg>,
        IPagedAsyncWalker<T, TArg>
    {
        private readonly Func<CancellationToken, TArg, Action<T>, Task> _singleWalkFunc;
        private readonly Func<CancellationToken, TArg, Func<CancellationToken, T, Task>, Task> _singleAsyncWalkFunc;
        private readonly Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task> _pagedWalkFunc;
        private readonly Func<CancellationToken, PageInfo, TArg, Func<CancellationToken, IEnumerable<T>, Task>, Task> _pagedAsyncWalkFunc;

        public Walker( 
            Func<CancellationToken, TArg, Action<T>, Task> singleWalkFunc,
            Func<CancellationToken, TArg, Func<CancellationToken, T, Task>, Task> singleAsyncWalkFunc,
            Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task> pagedWalkFunc,
            Func<CancellationToken, PageInfo, TArg, Func<CancellationToken, IEnumerable<T>, Task>, Task> pagedAsyncWalkFunc
            )
        {
            _singleWalkFunc = singleWalkFunc;
            _singleAsyncWalkFunc = singleAsyncWalkFunc;
            _pagedWalkFunc = pagedWalkFunc;
            _pagedAsyncWalkFunc = pagedAsyncWalkFunc;
        }
        
        async Task ISingleWalker<T, TArg>.WalkAsync(CancellationToken cancellationToken, TArg request, Action<T> callback) => await _singleWalkFunc(cancellationToken, request, callback);

        async Task IPagedWalker<T, TArg>.WalkAsync(CancellationToken cancellationToken, PageInfo pageInfo, TArg request, Action<IEnumerable<T>> callback) => await _pagedWalkFunc(cancellationToken, pageInfo, request, callback);

        async Task ISingleAsyncWalker<T, TArg>.WalkAsync(CancellationToken cancellationToken, TArg request, Func<CancellationToken, T, Task> callbackAsync) => await _singleAsyncWalkFunc(cancellationToken, request, callbackAsync);

        async Task IPagedAsyncWalker<T, TArg>.WalkAsync(CancellationToken cancellationToken, PageInfo pageInfo, TArg request, Func<CancellationToken, IEnumerable<T>, Task> callbackAsync) => await _pagedAsyncWalkFunc(cancellationToken, pageInfo, request, callbackAsync);
    }
}