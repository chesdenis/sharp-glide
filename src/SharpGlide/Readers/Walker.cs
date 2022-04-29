using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers
{
    public class Walker<T> : IWalker<T>
    {
        private readonly Func<CancellationToken, Action<T>, Task> _walkFunc;
        private readonly Func<CancellationToken, Func<CancellationToken, T, Task>, Task> _walkAsyncFunc;
        private readonly Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task> _walkPagedFunc;

        private readonly Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task>
            _walkPagedAsyncFunc;

        public Walker(
            Func<CancellationToken, Action<T>, Task> walkFunc,
            Func<CancellationToken, Func<CancellationToken, T, Task>, Task> walkAsyncFunc,
            Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task> walkPagedFunc,
            Func<CancellationToken, PageInfo, Func<CancellationToken, IEnumerable<T>, Task>, Task> walkPagedAsyncFunc)
        {
            _walkFunc = walkFunc;
            _walkAsyncFunc = walkAsyncFunc;
            _walkPagedFunc = walkPagedFunc;
            _walkPagedAsyncFunc = walkPagedAsyncFunc;
        }

        public async Task WalkAsync(CancellationToken cancellationToken, Action<T> callback) =>
            await _walkFunc(cancellationToken, callback);

        public async Task WalkAsync(CancellationToken cancellationToken, Func<CancellationToken, T, Task> callbackAsync)
            => await _walkAsyncFunc(cancellationToken, callbackAsync);

        public async Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo,
            Action<IEnumerable<T>> callback) =>
            await _walkPagedFunc(cancellationToken, pageInfo, callback);

        public async Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo,
            Func<CancellationToken, IEnumerable<T>, Task> callbackAsync)
            => await _walkPagedAsyncFunc(cancellationToken, pageInfo, callbackAsync);
    }
}