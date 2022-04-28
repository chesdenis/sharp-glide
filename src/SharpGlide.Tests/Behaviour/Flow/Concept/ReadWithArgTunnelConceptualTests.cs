using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
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
            var directReaderProxy = flowModel.BuildReader(intReader);
            var readSingle = await 
                directReaderProxy.ReadAsync(CancellationToken.None, SourceDataA.ElementAt(5000));
    
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
            var directReaderProxy = flowModel.BuildReader(intReader);
            var readRange = await directReaderProxy.ReadSpecific(CancellationToken.None,
                (ints => ints.Skip(10).Take(10)), SourceDataA.ElementAt(5000));
    
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
            var directReaderProxy = flowModel.BuildReader(intReader);
            var readAll = await directReaderProxy.ReadAllAsync(CancellationToken.None, SourceDataA.ElementAt(5000));
    
            // Assert
            readAll.Count().Should().Be(4999);
        }
    
       
    }
}