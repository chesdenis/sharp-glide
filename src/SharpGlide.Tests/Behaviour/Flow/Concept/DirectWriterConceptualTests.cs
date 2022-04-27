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
    public class DirectWriterConceptualTests
    {
        [Fact]
        public async Task WriterProxyShouldWriteSingleEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();

            var stringWriter = new StringWriter(() => output);
            flowModel.AddTunnel(model => stringWriter);

            // Act
            var directWriterProxy = flowModel.GetProxy(stringWriter);

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
            var directWriterProxy = flowModel.GetProxy(stringWriter);
    
            // Assert
            var retVal = await directWriterProxy.WriteAndReturnSingle("test1", Route.Default, CancellationToken.None);
    
            output.First().Should().StartWith("test1");
            retVal.Should().Contain("handled");
        }
    
        [Fact]
        public async Task WriterProxyShouldWriteRangeEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();
    
            var stringWriter = new StringWriter(() => output);
            flowModel.AddTunnel(model => stringWriter);
    
            // Act
            var directWriterProxy = flowModel.GetProxy(stringWriter);
    
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
            var directWriterProxy = flowModel.GetProxy(stringWriter);
    
            // Assert
            var result = await directWriterProxy.WriteAndReturnRange(new List<string>() { "a", "b", "c" }, Route.Default,
                CancellationToken.None);
    
            result.Count().Should().Be(3);
            result.First().Should().Contain("handled");
        }
    }
}