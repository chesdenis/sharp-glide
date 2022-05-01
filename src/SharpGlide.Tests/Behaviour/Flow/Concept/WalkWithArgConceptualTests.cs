using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Readers;
using SharpGlide.Tests.Behaviour.Flow.Concept.Examples;
using SharpGlide.Tunnels.Read.Model;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Flow.Concept
{
    public class WalkWithArgConceptualTests
    {
        private static List<int> SourceDataA = Enumerable.Range(0, 10000).ToList();
        
        [Fact]
        public async Task ShouldWalkThroughCollection()
        {
            // Arrange
            var flowModel = new FlowModel();
            var walker = new IntWalkWithArgTunnel(() => SourceDataA);
            flowModel.AddTunnel((model) => walker);

            var output = new List<int>();
     
            // Act
            var walkerProxy = flowModel.BuildWalker(walker) as ISingleWalker<int, int>;
            await walkerProxy.WalkAsync(CancellationToken.None, 5000, i => output.Add(i));
    
            // Assert
            output.Count.Should().Be(SourceDataA.Count/2 - 1);
        }

        [Fact]
        public async Task ShouldWalkPagedThroughCollection()
        { 
            // Arrange
            var flowModel = new FlowModel();
            var walker = new IntWalkWithArgTunnel(() => SourceDataA);
            flowModel.AddTunnel((model) => walker);

            var output = new List<List<int>>();
     
            // Act
            var walkerProxy = flowModel.BuildWalker(walker) as IPagedWalker<int, int>;
            var pageInfo = new PageInfo
            {
                PageIndex = 0,
                PageSize = 10
            };
            
            await walkerProxy.WalkAsync(CancellationToken.None, pageInfo, 5000,
                pageData => output.Add(pageData.ToList()));
    
            // Assert
            output.Count.Should().Be(SourceDataA.Count / (10 * 2));
        }
    }
}