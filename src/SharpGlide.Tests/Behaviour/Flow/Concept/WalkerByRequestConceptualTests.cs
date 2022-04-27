using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Tests.Behaviour.Flow.Concept.Examples;
using SharpGlide.Tunnels.Readers.Model;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Flow.Concept
{
    public class WalkerByRequestConceptualTests
    {
        private static List<int> SourceDataA = Enumerable.Range(0, 10000).ToList();
        
        [Fact]
        public async Task ShouldWalkThroughCollection()
        {
            // Arrange
            var flowModel = new Model();
            var walker = new IntWalkerByRequest(() => SourceDataA);
            flowModel.AddTunnel((model) => walker);

            var output = new List<int>();
     
            // Act
            var walkerProxy = flowModel.GetProxy(walker);
            await walkerProxy.WalkAsync(CancellationToken.None, 5000, i => output.Add(i));
    
            // Assert
            output.Count.Should().Be(SourceDataA.Count/2 - 1);
        }

        [Fact]
        public async Task ShouldWalkPagedThroughCollection()
        { 
            // Arrange
            var flowModel = new Model();
            var walker = new IntWalkerByRequest(() => SourceDataA);
            flowModel.AddTunnel((model) => walker);

            var output = new List<List<int>>();
     
            // Act
            var walkerProxy = flowModel.GetProxy(walker);
            var pageInfo = new PageInfo
            {
                PageIndex = 0,
                PageSize = 10
            };
            
            await walkerProxy.WalkPagedAsync(CancellationToken.None, pageInfo, 5000,
                pageData => output.Add(pageData.ToList()));
    
            // Assert
            output.Count.Should().Be(SourceDataA.Count / (10 * 2));
        }
    }
}