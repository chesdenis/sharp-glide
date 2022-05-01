using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Abstractions;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class StringCollectionWriteTunnel : CollectionWriteTunnel<string>
    {
        private readonly Func<List<string>> _storagePointer;
        
        public StringCollectionWriteTunnel(Func<List<string>> storagePointer)
        {
            _storagePointer = storagePointer;
        }
        
        protected override async Task WriteImpl(IEnumerable<string> dataRange, IRoute route,
            CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().AddRange(dataRange.Select(s => $"{s} -> {route}"));
        }

        protected override async Task<IEnumerable<string>> WriteAndReturnImpl(IEnumerable<string> dataRange,
            IRoute route, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().AddRange(dataRange.Select(s => $"{s} -> {route}"));

            var retVal = new List<string>();
            foreach (var s in dataRange)
            {
                retVal.Add(s + " - handled");
            }

            return await Task.FromResult(retVal);
        }
    }
}