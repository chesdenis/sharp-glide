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
    public class IntWalkWithArgTunnel : WalkTunnel<int, int>
    {
        private readonly Func<List<int>> _storagePointer;

        public IntWalkWithArgTunnel(Func<List<int>> storagePointer)
        {
            _storagePointer = storagePointer;
        }
         
        protected override async Task SingleWalkImpl(CancellationToken cancellationToken, int request, Action<int> callback)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            
            foreach (var dataRow in _storagePointer().Where(w => w > request))
            {
                callback(dataRow);
            }
        }

        protected override async Task SingleAsyncWalkImpl(CancellationToken cancellationToken, int request, Func<CancellationToken, int, Task> callback)
        {
            throw new NotImplementedException();
        }

        protected override async Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, int request, Action<IEnumerable<int>> callback)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            
            var filtered = _storagePointer().Where(w => w > request).ToList();
            
            for (var i = 0; i < filtered.Count; i += pageInfo.PageSize)
            {
                callback(filtered.Skip(i).Take(pageInfo.PageSize));
                pageInfo.PageIndex++;
            }
        }

        protected override async Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, int request, Func<CancellationToken, IEnumerable<int>, Task> callback)
        {
            throw new NotImplementedException();
        }
    }
}