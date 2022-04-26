using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public class DirectWalkerProxy<T> : IDirectWalkerProxy<T>
    {
        private readonly Func<CancellationToken, Task<Action<T>>> _walkFunc;
        private readonly Func<CancellationToken, Task<Action<IEnumerable<T>>>> _walkPagedFunc;

        public DirectWalkerProxy(Func<CancellationToken, Task<Action<T>>> walkFunc,
            Func<CancellationToken, Task<Action<IEnumerable<T>>>> walkPagedFunc)
        {
            _walkFunc = walkFunc;
            _walkPagedFunc = walkPagedFunc;
        }

        public async Task<Action<T>> WalkAsync(CancellationToken cancellationToken) =>
            await _walkFunc(cancellationToken);

        public async Task<Action<IEnumerable<T>>> WalkPagedAsync(CancellationToken cancellationToken) =>
            await _walkPagedFunc(cancellationToken);
    }
}