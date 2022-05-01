using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Readers;
using SharpGlide.Tests.Behaviour.Flow.Concept.Examples;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Flow.Concept
{
    public class ReadWithArgTunnelConceptualTests
    {
        private static List<int> SourceDataA = Enumerable.Range(0, 10000).ToList();

        [Fact]
        public async Task ShouldGetSingleEntry()
        {
            // Arrange
            var flowModel = new FlowModel();
            var intReader = new IntReadWithArgTunnel(() => SourceDataA);
            flowModel.AddTunnel((model) => intReader);
     
            // Act
            var reader = flowModel.BuildReader(intReader) as ISingleReader<int, int>;
            var readSingle = await 
                reader.ReadAsync(CancellationToken.None, SourceDataA.ElementAt(5000));
    
            // Assert
            readSingle.Should().Be(5001);
        }
    
        [Fact]
        public async Task ShouldGetRangeEntries()
        {
            // Arrange
            var flowModel = new FlowModel();
            var intReader = new IntReadWithArgTunnel(() => SourceDataA);
            flowModel.AddTunnel((model) => intReader);
    
            // Act
            var directReaderProxy = flowModel.BuildReader(intReader) as IFilteredReader<int, int>;
            var readRange = await directReaderProxy.ReadAsync(CancellationToken.None,
                SourceDataA.ElementAt(5000), ints => ints.Skip(10).Take(10));
    
            // Assert
            readRange.Count().Should().Be(10);
            readRange.First().Should().Be(5011);
            readRange.Last().Should().Be(5020);
        }
    
        [Fact]
        public async Task ShouldGetAllEntries()
        {
            // Arrange
            var flowModel = new FlowModel(); 
            var intReader = new IntReadWithArgTunnel(() => SourceDataA);
            flowModel.AddTunnel((model) => intReader);
    
            // Act
            var directReaderProxy = flowModel.BuildReader(intReader) as ICollectionReader<int, int>;
            var readAll = await directReaderProxy.ReadAsync(CancellationToken.None, SourceDataA.ElementAt(5000));
    
            // Assert
            readAll.Count().Should().Be(4999);
        }
    
       
    }
}