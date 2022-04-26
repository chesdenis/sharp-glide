using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Proxy
{
    public class DirectWalkerProxy<T> : IDirectWalkerProxy<T>
    {
        private readonly Func<CancellationToken, Action<T>, Task> _walkFunc;
        
        private readonly Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task> _walkPagedFunc;
        
        public DirectWalkerProxy(
            Func<CancellationToken, Action<T>, Task> walkFunc, 
            Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task> walkPagedFunc)
        {
            _walkFunc = walkFunc;
            _walkPagedFunc = walkPagedFunc;
        }

        public async Task WalkAsync(CancellationToken cancellationToken, Action<T> callback) =>
            await _walkFunc(cancellationToken, callback);

        public async Task WalkPagedAsync(CancellationToken cancellationToken, PageInfo pageInfo, Action<IEnumerable<T>> callback) =>
            await _walkPagedFunc(cancellationToken, pageInfo, callback);
    }
}