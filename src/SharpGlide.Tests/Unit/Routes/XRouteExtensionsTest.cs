using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Tunnels.Routes.XRoutes;
using Xunit;

namespace SharpGlide.Tests.Unit.Routes
{
    public class XRouteExtensionsTest
    {
        [Fact]
        public async Task ShouldBuildDefaultRoutes()
        {
            // Arrange

            // Act
            Dashboard.Create()
                .TakeParts(1, 2, 3).FlowFromSelf()
                .TakePart(2).FlowTo(x => x.TakePart(3))
                .TakePart(3).FlowTo(x => x.TakePart(1));

            Dashboard.Create()
                .TakeParts(1).FlowFromSelf()
                .TakePart(1).FlowTo(x => x.TakePart(2))
                .TakePart(1).FlowTo(x => x.TakePart(3))
                .TakePart(1).FlowTo(x => x.TakePart(4))
                .TakeParts(1, 2, 3, 4, 5).Start();

            Dashboard.Create()
                .TakeParts(1, 2, 3).FlowFromSelf()
                .TakePart(1).FlowTo(x => x.TakeParts(2, 3, 4));

            Dashboard.Create()
                .TakeParts(1, 2, 3).Scale(100).FlowFromSelf()
                .TakePart(1).FlowTo(x => x.TakeParts(2, 3, 4));

            Dashboard.Create()
                .TakeParts(1, 2, 3).Scale(100).FlowFromSelf()
                .TakePart(1).FlowTo(x => x.TakeParts(2, 3, 4), "abcde");

            Dashboard.Create()
                .TakeParts(1, 2, 3).Scale(100).FlowFromSelf()
                .TakePart(1).FlowTo(x => x.TakeParts(2, 3, 4),
                    "topic", "abcde");
            
            Dashboard.Create()
                .TakeParts(1, 2, 3).Scale(100).FlowFromSelf()
                .TakePart(1).FlowFrom(x => x.TakeParts(2, 3, 4),
                    "queue", "abcde");

            // Assert
        }
        //
        // [Fact]
        // public async Task ShouldSupportCustomFlowFromSelf()
        // {
        //     // Arrange
        //
        //     // Act
        //     var consumeRoute = default(XConsumeRoute).FromSelf("CustomQueue");
        //
        //     // Assert
        //     consumeRoute.Queue.Should().Be("CustomQueue");
        // }
    }
}