using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public class WalkerByRequestProxy<T, TRequest> : IWalkerByRequestProxy<T, TRequest>
    {
        private readonly Func<CancellationToken, TRequest, Action<T>, Task> _walkFunc;
        private readonly Func<CancellationToken, PageInfo, TRequest, Action<IEnumerable<T>>, Task> _walkPagedFunc;

        public WalkerByRequestProxy(Func<CancellationToken, TRequest, Action<T>, Task> walkFunc,
            Func<CancellationToken, PageInfo, TRequest, Action<IEnumerable<T>>, Task> walkPagedFunc)
        {
            _walkFunc = walkFunc;
            _walkPagedFunc = walkPagedFunc;
        }

        public async Task WalkAsync(CancellationToken cancellationToken, TRequest request, Action<T> callback)
            => await _walkFunc(cancellationToken, request, callback);

        public async Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, TRequest request,
            Action<IEnumerable<T>> callback)
            => await _walkPagedFunc(cancellationToken, pageInfo, request, callback);
    }
}