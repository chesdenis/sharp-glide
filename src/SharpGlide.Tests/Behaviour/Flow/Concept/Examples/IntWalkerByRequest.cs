using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Abstractions;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntWalkerByRequest : WalkerByRequest<int, int>
    {
        private readonly Func<List<int>> _storagePointer;

        public IntWalkerByRequest(Func<List<int>> storagePointer)
        {
            _storagePointer = storagePointer;
        }
        
        protected override async Task WalkExprImpl(CancellationToken cancellationToken, int request, Action<int> callback)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            foreach (var dataRow in _storagePointer().Where(w => w > request))
            {
                callback(dataRow);
            }
        }

        protected override async Task WalkPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo, int request,
            Action<IEnumerable<int>> callback)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            var filtered = _storagePointer().Where(w => w > request).ToList();

            for (var i = 0; i < filtered.Count; i += pageInfo.PageSize)
            {
                callback(filtered.Skip(i).Take(pageInfo.PageSize));
                pageInfo.PageIndex++;
            }
        }
    }
}