using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Tests.Behaviour.Flow.Concept.Examples;
using SharpGlide.Tunnels.Read.Model;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Flow.Concept
{
    public class WalkTunnelConceptualTests
    {
        private static List<int> SourceDataA = Enumerable.Range(0, 10000).ToList();
        
        [Fact]
        public async Task ShouldWalkThroughCollection()
        {
            // Arrange
            var flowModel = new FlowModel();
            var walker = new IntWalkTunnel(() => SourceDataA);
            flowModel.AddTunnel((model) => walker);

            var output = new List<int>();
     
            // Act
            var walkerProxy = flowModel.BuildWalker(walker);
            await walkerProxy.WalkAsync(CancellationToken.None, i => output.Add(i));
    
            // Assert
            output.Count.Should().Be(SourceDataA.Count);
        }

        [Fact]
        public async Task ShouldWalkPagedThroughCollection()
        { 
            // Arrange
            var flowModel = new FlowModel();
            var walker = new IntWalkTunnel(() => SourceDataA);
            flowModel.AddTunnel((model) => walker);

            var output = new List<List<int>>();
     
            // Act
            var walkerProxy = flowModel.BuildWalker(walker);
            var pageInfo = new PageInfo
            {
                PageIndex = 0,
                PageSize = 10
            };
            
            await walkerProxy.WalkPagedAsync(CancellationToken.None, pageInfo, 
                pageData => output.Add(pageData.ToList()));
    
            // Assert
            output.Count.Should().Be(SourceDataA.Count / 10);
        }
    }
}