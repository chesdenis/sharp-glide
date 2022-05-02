using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Routing;
using SharpGlide.Tests.Behaviour.Flow.Concept.Examples;
using SharpGlide.Writers;
using SharpGlide.Writers.Interfaces;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Flow.Concept
{
    public class WriteTunnelConceptualTests
    {
        [Fact]
        public async Task ShouldWriteSingleEntry()
        {
            // Arrange
            var flowModel = new FlowModel();
            var output = new List<string>();

            var stringWriter = new StringSingleWriteTunnel(() => output);
            flowModel.AddTunnel(model => stringWriter);

            // Act
            var writer = flowModel.BuildSingleWriter(stringWriter) as ISingleWriter<string>;

            // Assert
            await writer.WriteSingle("test1", Route.Default, CancellationToken.None);
    
            output.First().Should().StartWith("test1");
        }
    
        [Fact]
        public async Task ShouldWriteAndReturnSingleEntry()
        {
            // Arrange
            var flowModel = new FlowModel();
            var output = new List<string>();
    
            var stringWriter = new StringSingleWriteTunnel(() => output);
            flowModel.AddTunnel(model => stringWriter);
    
            // Act
            var writer = flowModel.BuildSingleWriter(stringWriter)  as ISingleWriter<string>;;
    
            // Assert
            var retVal = await writer.WriteAndReturnSingle("test1", Route.Default, CancellationToken.None);
    
            output.First().Should().StartWith("test1");
            retVal.Should().Contain("handled");
        }
    
        [Fact]
        public async Task ShouldWriteRangeEntry()
        {
            // Arrange
            var flowModel = new FlowModel();
            var output = new List<string>();
    
            var stringWriter = new StringCollectionWriteTunnel(() => output);
            flowModel.AddTunnel(model => stringWriter);
    
            // Act
            var writer = flowModel.BuildCollectionWriter(stringWriter)  as ICollectionWriter<string>;
    
            // Assert
            await writer.WriteCollection(new List<string>() { "a", "b", "c" }, Route.Default, CancellationToken.None);
    
            output.Count.Should().Be(3);
        }
    
        [Fact]
        public async Task ShouldWriteAndReturnRangeEntry()
        {
            // Arrange
            var flowModel = new FlowModel();
            var output = new List<string>();
    
            var stringWriter = new StringCollectionWriteTunnel(() => output);
            flowModel.AddTunnel(model => stringWriter);
    
            // Act
            var directWriterProxy = flowModel.BuildCollectionWriter(stringWriter)  as ICollectionWriter<string>;;;
    
            // Assert
            var result = await directWriterProxy.WriteAndReturnCollection(new List<string>() { "a", "b", "c" }, Route.Default,
                CancellationToken.None);
    
            result.Count().Should().Be(3);
            result.First().Should().Contain("handled");
        }
    }
}