using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Writers;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class StringWriter : IOnDemandWriterV2<string>
    {
        private readonly Func<List<string>> _storagePointer;
        public bool CanExecute { get; set; }

        public StringWriter(Func<List<string>> storagePointer)
        {
            _storagePointer = storagePointer;
        }

        private async Task WriteSingleImpl(string data, IRoute route, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().Add($"{data} -> {route}");
        }

        private async Task WriteRangeImpl(IEnumerable<string> collection, IRoute route,
            CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().AddRange(collection.Select(s => $"{s} -> {route}"));
        }
            
        private async Task<IEnumerable<string>> WriteAndReturnRangeImpl(IEnumerable<string> collection, IRoute route,
            CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().AddRange(collection.Select(s => $"{s} -> {route}"));

            var retVal = new List<string>();
            foreach (var s in collection)
            {
                retVal.Add(s +" - handled");
            }

            return await Task.FromResult(retVal);
        }

        private async Task<string> WriteAndReturnSingleImpl(string data, IRoute route,
            CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().Add(data);

            return await Task.FromResult($"{data} - handled!");
        }

        public Expression<Func<string, IRoute, CancellationToken, Task>> WriteSingleExpr =>
            (data, route, token) => WriteSingleImpl(data, route, token);

        public Expression<Func<string, IRoute, CancellationToken, Task<string>>> WriteAndReturnSingleExpr =>
            (data, route, token) => WriteAndReturnSingleImpl(data, route, token);

        public Expression<Func<IEnumerable<string>, IRoute, CancellationToken, Task>> WriteRangeExpr =>
            (collection, route, token) => WriteRangeImpl(collection, route, token);

        public Expression<Func<IEnumerable<string>, IRoute, CancellationToken, Task<IEnumerable<string>>>>
            WriteAndReturnRangeExpr =>
            (collection, route, token) => WriteAndReturnRangeImpl(collection, route, token);
    }
}