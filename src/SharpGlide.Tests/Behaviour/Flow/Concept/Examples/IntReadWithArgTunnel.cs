using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Readers;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntReadWithArgTunnel : ReadTunnel<int, int>
    {
        private readonly Func<List<int>> _storagePointer;

        public IntReadWithArgTunnel(Func<List<int>> storagePointer)
        {
            _storagePointer = storagePointer;
        }
 
        protected override async Task<int> SingleReadImpl(CancellationToken cancellationToken, int arg)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return _storagePointer().First(w => w > arg);
        }

        protected override async Task<IEnumerable<int>> CollectionReadImpl(CancellationToken cancellationToken, int arg)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return _storagePointer().Where(w => w > arg).ToList();
        }

        protected override async Task<IEnumerable<int>> PagedReadImpl(CancellationToken cancellationToken,
            PageInfo pageInfo, int arg)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            var itemsToSkip = pageInfo.PageIndex * pageInfo.PageSize;
            var itemsToTake = pageInfo.PageSize;

            var filteredData = _storagePointer().Where(w => w > arg);

            if (itemsToSkip >= filteredData.Count())
            {
                return await Task.FromResult(filteredData);
            }

            return await Task.FromResult(filteredData.Skip(itemsToSkip).Take(itemsToTake));
        }

        protected override async Task<IEnumerable<int>> FilteredReadImpl(CancellationToken cancellationToken, int arg,
            Func<IEnumerable<int>, IEnumerable<int>> filter)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return filter(_storagePointer().Where(w => w > arg));
        }
    }
}