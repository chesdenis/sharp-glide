using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Writers.Abstractions;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class StringWriter : DirectWriter<string>
    {
        private readonly Func<List<string>> _storagePointer;
        
        public StringWriter(Func<List<string>> storagePointer)
        {
            _storagePointer = storagePointer;
        }

        protected override async Task WriteSingleImpl(string data, IRoute route, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().Add($"{data} -> {route}");
        }

        protected override async Task<string> WriteAndReturnSingleImpl(string data, IRoute route,
            CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().Add(data);

            return await Task.FromResult($"{data} - handled!");
        }

        protected override async Task WriteRangeImpl(IEnumerable<string> dataRange, IRoute route,
            CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().AddRange(dataRange.Select(s => $"{s} -> {route}"));
        }

        protected override async Task<IEnumerable<string>> WriteAndReturnRangeImpl(IEnumerable<string> dataRange,
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