using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts;
using SharpGlide.Routing;

namespace SharpGlide.Tests.Behaviour.Parts.Concept.Examples
{
    public class TwoInTwoOutPart : IBasePart
    {
        private readonly Func<Task<decimal>> _sourceA;
        private readonly Func<Task<IEnumerable<decimal>>> _sourceBRange;
        private readonly Func<decimal, Route, Task> _writeActionA;
        private readonly Func<IEnumerable<decimal>, IRoute, Task> _writeRangeActionB;

        public TwoInTwoOutPart(
            Func<Task<decimal>> sourceA,
            Func<Task<IEnumerable<decimal>>> sourceBRange,
            Func<decimal, Route, Task> writeActionA,
            Func<IEnumerable<decimal>, IRoute, Task> writeRangeActionB
        )
        {
            _sourceA = sourceA;
            _sourceBRange = sourceBRange;
            _writeActionA = writeActionA;
            _writeRangeActionB = writeRangeActionB;
        }

        public string Name { get; set; } = "TwoInTwoOut!";

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            var data = await _sourceA();
            var rangeOfData = await _sourceBRange();

            await _writeActionA(data, Route.Default);
            await _writeRangeActionB(rangeOfData, Route.Default);
        }
    }
}