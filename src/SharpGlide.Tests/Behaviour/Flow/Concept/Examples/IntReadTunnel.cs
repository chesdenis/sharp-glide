using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntReadTunnel : ReadTunnel<int>
    {
        private readonly Func<List<int>> _storagePointer;

        public IntReadTunnel(Func<List<int>> storagePointer)
        {
            _storagePointer = storagePointer;
        }

        protected override async Task<int> ReadSingleImpl(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return _storagePointer().First();
        }

        protected override async Task<IEnumerable<int>> ReadPagedImpl(CancellationToken cancellationToken,
            PageInfo pageInfo)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            var itemsToSkip = pageInfo.PageIndex * pageInfo.PageSize;
            var itemsToTake = pageInfo.PageSize;

            if (itemsToSkip >= _storagePointer().Count)
            {
                return await Task.FromResult(_storagePointer());
            }

            return await Task.FromResult(_storagePointer().Skip(itemsToSkip).Take(itemsToTake));
        }

        protected override async Task<IEnumerable<int>> ReadSpecificImpl(CancellationToken cancellationToken,
            Func<IEnumerable<int>, IEnumerable<int>> filter)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return filter(_storagePointer());
        }

        protected override async Task<IEnumerable<int>> ReadAllImpl(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return _storagePointer().ToList();
        }
    }
}