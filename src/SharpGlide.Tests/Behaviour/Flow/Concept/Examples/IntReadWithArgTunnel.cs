using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntReadWithArgTunnel : ReadWithArgTunnel<int, int>
    {
        private readonly Func<List<int>> _storagePointer;

        public IntReadWithArgTunnel(Func<List<int>> storagePointer)
        {
            _storagePointer = storagePointer;
        }
        protected override async Task<int> ReadSingleImpl(CancellationToken cancellationToken, int request)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return _storagePointer().First(w => w > request);
        }

        protected override async Task<IEnumerable<int>> ReadAllImpl(CancellationToken cancellationToken, int request)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return _storagePointer().Where(w => w > request).ToList();
        }

        protected override async Task<IEnumerable<int>> ReadPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo, int request)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            var itemsToSkip = pageInfo.PageIndex * pageInfo.PageSize;
            var itemsToTake = pageInfo.PageSize;

            var filteredData = _storagePointer().Where(w => w > request);

            if (itemsToSkip >= filteredData.Count())
            {
                return await Task.FromResult(filteredData);
            }

            return await Task.FromResult(filteredData.Skip(itemsToSkip).Take(itemsToTake));
        }

        protected override async Task<IEnumerable<int>> ReadSpecificImpl(CancellationToken cancellationToken, int request, Func<IEnumerable<int>, IEnumerable<int>> filter)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return filter(_storagePointer().Where(w=>w>request));
        }
    }
}