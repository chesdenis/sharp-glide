using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Readers
{
    public class WalkerWithArg<T, TArg> : IWalkerWithArg<T, TArg>
    {
        private readonly Func<CancellationToken, TArg, Action<T>, Task> _walkFunc;
        private readonly Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task> _walkPagedFunc;

        public WalkerWithArg(Func<CancellationToken, TArg, Action<T>, Task> walkFunc,
            Func<CancellationToken, PageInfo, TArg, Action<IEnumerable<T>>, Task> walkPagedFunc)
        {
            _walkFunc = walkFunc;
            _walkPagedFunc = walkPagedFunc;
        }

        public async Task WalkAsync(CancellationToken cancellationToken, TArg request, Action<T> callback)
            => await _walkFunc(cancellationToken, request, callback);

        public async Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, TArg request,
            Action<IEnumerable<T>> callback)
            => await _walkPagedFunc(cancellationToken, pageInfo, request, callback);
    }
}