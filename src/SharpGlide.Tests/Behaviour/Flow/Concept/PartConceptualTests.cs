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
    public class PartConceptualTests
    {
        private static List<int> SourceDataA = Enumerable.Range(2, 10000).ToList();
    
        [Fact]
        public async Task IntToStringFlowShouldCalculateStrings()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();
            
            var intReader = new IntReader(() => SourceDataA);
            var stringWriter = new StringWriter(() => output);

            flowModel.AddTunnel(model => intReader);
            flowModel.AddTunnel(model => stringWriter);

            var part = new IntToStringPart(
                flowModel.GetProxy(intReader), 
                flowModel.GetProxy(stringWriter));
    
            flowModel.Parts.Add("int2string", part);
    
            // Act
            await part.ProcessAsync(CancellationToken.None);
    
            // Assert
            output.Should().HaveCount(1);
            output.First().Should().StartWith("4");
        }
    }
}