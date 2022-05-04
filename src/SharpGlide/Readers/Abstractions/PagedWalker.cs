using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Readers.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers.Abstractions
{
    public class PagedWalker<T, TArg> :
        IPagedWalker<T, TArg>,
        IPagedAsyncWalker<T, TArg>
    {
        private readonly Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task> _pagedWalkFunc;
        private readonly Func<CancellationToken, PageInfo, TArg, Func<CancellationToken, IEnumerable<T>, Task>, Task> _pagedAsyncWalkFunc;

        public PagedWalker(
            Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task> pagedWalkFunc,
            Func<CancellationToken, PageInfo, TArg, Func<CancellationToken, IEnumerable<T>, Task>, Task> pagedAsyncWalkFunc
        )
        {
            _pagedWalkFunc = pagedWalkFunc;
            _pagedAsyncWalkFunc = pagedAsyncWalkFunc;
        }

        public async Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, TArg request, Action<IEnumerable<T>> callback) => await _pagedWalkFunc(cancellationToken, pageInfo, request, callback);

        public async Task WalkAsyncPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, TArg request, Func<CancellationToken, IEnumerable<T>, Task> callbackAsync) => await _pagedAsyncWalkFunc(cancellationToken, pageInfo, request, callbackAsync);
    }

    public class PagedWalker<T> :
        IPagedWalker<T>,
        IPagedAsyncWalker<T>
    {
        private readonly Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task> _pagedWalkFunc;

        private readonly Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task>
            _pagedAsyncWalkFunc;

        public PagedWalker(
            Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task> pagedWalkFunc,
            Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task> pagedAsyncWalkFunc)
        {
            _pagedWalkFunc = pagedWalkFunc;
            _pagedAsyncWalkFunc = pagedAsyncWalkFunc;
        }

        public async Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo,
            Action<IEnumerable<T>> callback) => await _pagedWalkFunc(cancellationToken, pageInfo, callback);

        public async Task WalkAsyncPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo,
            Func<CancellationToken, IEnumerable<T>, Task> callbackAsync) =>
            await _pagedAsyncWalkFunc(cancellationToken, pageInfo, callbackAsync);
    }
}