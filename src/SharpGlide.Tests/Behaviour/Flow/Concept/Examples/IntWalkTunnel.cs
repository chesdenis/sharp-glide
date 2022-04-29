using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntWalkTunnel : WalkTunnel<int>
    {
        private readonly Func<List<int>> _storagePointer;

        public IntWalkTunnel(Func<List<int>> storagePointer)
        {
            _storagePointer = storagePointer;
        }

        protected override async Task WalkImpl(CancellationToken cancellationToken, Action<int> callback)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            foreach (var dataRow in _storagePointer())
            {
                callback(dataRow);
            }
        }

        protected override Task WalkAsyncImpl(CancellationToken cancellationToken, Func<CancellationToken, int, Task> callback)
        {
            throw new NotImplementedException();
        }


        protected override async Task WalkPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Action<IEnumerable<int>> callback)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            for (var i = 0; i < _storagePointer().Count; i += pageInfo.PageSize)
            {
                callback(_storagePointer().Skip(i).Take(pageInfo.PageSize));
                pageInfo.PageIndex++;
            }
        }

        protected override Task WalkPagedAsyncImpl(CancellationToken cancellationToken, PageInfo pageInfo, Func<CancellationToken, IEnumerable<int>, Task> callback)
        {
            throw new NotImplementedException();
        }
    }
}