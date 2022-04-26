using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Routing;
using SharpGlide.Tests.Behaviour.Flow.Concept.Examples;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Flow.Concept
{
    public class ReaderAndWriterConceptualTests
    {
        public static List<int> SourceDataA = Enumerable.Range(0, 10000).ToList();

        [Fact]
        public async Task ReaderProxyShouldGetSingleEntry()
        {
            // Arrange
            var flowModel = new Model();
            var intReader = new IntReader(() => SourceDataA);
            flowModel.AddTunnel((model) => intReader);
     
            // Act
            var directReaderProxy = flowModel.GetDirectReaderProxy(intReader);
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
            var directReaderProxy = flowModel.GetDirectReaderProxy(intReader);
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
            var directReaderProxy = flowModel.GetDirectReaderProxy(intReader);
            var readAll = await directReaderProxy.ReadAllAsync(CancellationToken.None);
    
            // Assert
            readAll.Count().Should().Be(10000);
        }
    
        [Fact]
        public async Task WriterProxyShouldWriteSingleEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();

            var stringWriter = new StringWriter(() => output);
            flowModel.AddTunnel(model => stringWriter);

            // Act
            var directWriterProxy = flowModel.GetDirectWriterProxy(stringWriter);

            // Assert
            await directWriterProxy.WriteSingle("test1", Route.Default, CancellationToken.None);
    
            output.First().Should().StartWith("test1");
        }
    
        [Fact]
        public async Task WriterProxyShouldWriteAndReturnSingleEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();
    
            var stringWriter = new StringWriter(() => output);
            flowModel.AddTunnel(model => stringWriter);
    
            // Act
            var directWriterProxy = flowModel.GetDirectWriterProxy(stringWriter);
    
            // Assert
            var retVal = await directWriterProxy.WriteAndReturnSingle("test1", Route.Default, CancellationToken.None);
    
            output.First().Should().StartWith("test1");
            retVal.Should().Contain("handled");
        }
    
        [Fact]
        public async Task OWriterProxyShouldWriteRangeEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();
    
            var stringWriter = new StringWriter(() => output);
            flowModel.AddTunnel(model => stringWriter);
    
            // Act
            var directWriterProxy = flowModel.GetDirectWriterProxy(stringWriter);
    
            // Assert
            await directWriterProxy.WriteRange(new List<string>() { "a", "b", "c" }, Route.Default, CancellationToken.None);
    
            output.Count.Should().Be(3);
        }
    
        [Fact]
        public async Task WriterProxyShouldWriteAndReturnRangeEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();
    
            var stringWriter = new StringWriter(() => output);
            flowModel.AddTunnel(model => stringWriter);
    
            // Act
            var directWriterProxy = flowModel.GetDirectWriterProxy(stringWriter);
    
            // Assert
            var result = await directWriterProxy.WriteAndReturnRange(new List<string>() { "a", "b", "c" }, Route.Default,
                CancellationToken.None);
    
            result.Count().Should().Be(3);
            result.First().Should().Contain("handled");
        }
    }
}