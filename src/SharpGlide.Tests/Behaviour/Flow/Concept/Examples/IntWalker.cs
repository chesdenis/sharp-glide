using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Abstractions;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntWalker : DirectWalker<int>
    {
        private readonly Func<List<int>> _storagePointer;

        public IntWalker(Func<List<int>> storagePointer)
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
    }
}