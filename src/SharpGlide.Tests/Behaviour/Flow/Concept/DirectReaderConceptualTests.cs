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
    public class DirectReaderConceptualTests
    {
        private static List<int> SourceDataA = Enumerable.Range(0, 10000).ToList();

        [Fact]
        public async Task ReaderProxyShouldGetSingleEntry()
        {
            // Arrange
            var flowModel = new Model();
            var intReader = new IntReader(() => SourceDataA);
            flowModel.AddTunnel((model) => intReader);
     
            // Act
            var directReaderProxy = flowModel.GetProxy(intReader);
            var readSingle = await directReaderProxy.ReadAsync(CancellationToken.None);
    
            // Assert
            readSingle.Should().Be(0);
        }
    
        [Fact]
        public async Task ReaderProxyShouldGetRangeEntries()
        {
            // Arrange
            var flowModel = new Model();
            var intReader = new IntReader(() => SourceDataA);
            flowModel.AddTunnel((model) => intReader);
    
            // Act
            var directReaderProxy = flowModel.GetProxy(intReader);
            var readRange = await directReaderProxy.ReadSpecific(CancellationToken.None,
                (ints => ints.Skip(10).Take(10)));
    
            // Assert
            readRange.Count().Should().Be(10);
            readRange.First().Should().Be(10);
            readRange.Last().Should().Be(19);
        }
    
        [Fact]
        public async Task ReaderProxyShouldGetAllEntries()
        {
            // Arrange
            var flowModel = new Model(); 
            var intReader = new IntReader(() => SourceDataA);
            flowModel.AddTunnel((model) => intReader);
    
            // Act
            var directReaderProxy = flowModel.GetProxy(intReader);
            var readAll = await directReaderProxy.ReadAllAsync(CancellationToken.None);
    
            // Assert
            readAll.Count().Should().Be(10000);
        }
    
       
    }
}