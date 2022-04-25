using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntReader : IOnDemandReaderV2<int>
    {
        private readonly Func<List<int>> _storagePointer;
        public bool CanExecute { get; set; }
        public Expression<Func<CancellationToken, Task<int>>> ReadSingleExpr => (token) => ReadSingleImpl(token);

        public Expression<Func<CancellationToken, Task<IEnumerable<int>>>> ReadAllExpr =>
            (token) => ReadAllImpl(token);

        public Expression<Func<CancellationToken, Func<IEnumerable<int>, IEnumerable<int>>, Task<IEnumerable<int>>>>
            ReadRangeExpr =>
            (token, filterPointer) => ReadRangeImpl(token, filterPointer);

        public IntReader(Func<List<int>> storagePointer)
        {
            _storagePointer = storagePointer;
        }

        private async Task<int> ReadSingleImpl(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return _storagePointer().First();
        }

        private async Task<IEnumerable<int>> ReadAllImpl(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return _storagePointer().ToList();
        }

        private async Task<IEnumerable<int>> ReadRangeImpl(CancellationToken cancellationToken,
            Func<IEnumerable<int>, IEnumerable<int>> filterPointer)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            return filterPointer(_storagePointer());
        }
    }
}