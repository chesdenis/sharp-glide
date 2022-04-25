using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.FlowSchema;
using SharpGlide.Tunnels.Readers;
using Xunit;


namespace SharpGlide.Tests.Behaviour.FlowSchema.Concept
{
    public class FlowSchemaConceptualTests
    {
        public static List<int> SourceDataA = Enumerable.Range(0, 10000).ToList();
        public static List<int> SourceDataB = Enumerable.Range(0, 10000).ToList();
        public static List<int> SourceDataC = Enumerable.Range(0, 10000).ToList();

        [Fact]
        public async Task OnDemandReaderShouldGetSingleEntry()
        {
            // Arrange
            var flowModel = new Model();

            flowModel.Tunnels.Add($"{nameof(SourceDataA)}Reader", new IntReader(() => SourceDataA));

            var intReader = flowModel.Tunnels[$"{nameof(SourceDataA)}Reader"] as IntReader;
            
            // Act
            var readSingle = await intReader.ReadSingleExpr.Compile()(CancellationToken.None);
            var readAll = await intReader.ReadAllExpr.Compile()(CancellationToken.None);
            var readRangeFunc =  intReader.ReadRangeExpr.Compile();
            var readRange = await readRangeFunc(CancellationToken.None, 
                (ints => ints.Skip(10).Take(10)));

            // Assert
            readSingle.Should().Be(0);
            readAll.Count().Should().Be(10000);
            readRange.Count().Should().Be(10);
            readRange.First().Should().Be(10);
            readRange.Last().Should().Be(19);
        }
        
        [Fact]
        public async Task OnDemandReaderShouldProcessSingleRead()
        {
            // Arrange
            var flowModel = new Model();

            flowModel.Tunnels.Add($"{nameof(SourceDataA)}Reader", new IntReader(() => SourceDataA));

            var intReader = flowModel.Tunnels[$"{nameof(SourceDataA)}Reader"] as IntReader;
            
            // Act
            var readSingle = await intReader.ReadSingleExpr.Compile()(CancellationToken.None);
            var readAll = await intReader.ReadAllExpr.Compile()(CancellationToken.None);
            var readRangeFunc =  intReader.ReadRangeExpr.Compile();
            var readRange = await readRangeFunc(CancellationToken.None, 
                (ints => ints.Skip(10).Take(10)));

            // Assert
            readSingle.Should().Be(0);
            readAll.Count().Should().Be(10000);
            readRange.Count().Should().Be(10);
            readRange.First().Should().Be(10);
            readRange.Last().Should().Be(19);
        }

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
}