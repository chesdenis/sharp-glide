using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Orchestration;
using SharpGlide.Tests.Model.VectorPart;
using Xunit;
using Xunit.Abstractions;

namespace SharpGlide.Tests.Unit.Routes
{
    public class XRouteExtensionsTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XRouteExtensionsTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ShouldBuildFlowFromSelf()
        {
            // Arrange
            var a = new TestVectorPart();
            var b = new TestVectorPart();
            var c = new TestVectorPart();

            // Act
            var dashboard = Dashboard.Create();

            dashboard
                .SelectParts(a, b, c).FlowFromSelf();

            // Assert
            dashboard.EnumerateRoutes().Should().NotBeEmpty();
            dashboard.EnumerateRoutes().Should().HaveCount(1);
            var routeLink = dashboard.EnumerateRoutes().First();

            var xConsumeRoute = routeLink.ConsumeRoute;
            xConsumeRoute.Queue.Should().Be("Unnamed TestVectorPart->[selfQueue]");
            xConsumeRoute.RoutingKey.Should().Be("#");

            xConsumeRoute.RouteAssignments.Should().HaveCount(3);

            routeLink.PublishRoute.Should().BeNull();
        }

        [Fact]
        public async Task ShouldBuildFlowTo()
        {
            // Arrange
            var a = new TestVectorPart();
            var b = new TestVectorPart();
            var c = new TestVectorPart();
        
            a.Name = nameof(a);
            b.Name = nameof(b);
            c.Name = nameof(c);
        
            // Act
            var dashboard = Dashboard.Create();
        
            dashboard
                .SelectParts(a).FlowFromSelf()
                .SelectParts(a).FlowTo(b)
                .SelectParts(a).FlowTo(c);
        
            // Assert
            dashboard.EnumerateRoutes().Should().NotBeEmpty();
            dashboard.EnumerateRoutes().Should().HaveCount(4);
            var routes = dashboard.EnumerateRoutes().ToList();
            foreach (var rt in routes)
            {
                _testOutputHelper.WriteLine(rt.ToString());
            }
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

        //
        // Dashboard.Create()
        //     .TakeParts(1).FlowFromSelf()
        //     .TakePart(1).FlowTo(x => x.TakePart(2))
        //     .TakePart(1).FlowTo(x => x.TakePart(3))
        //     .TakePart(1).FlowTo(x => x.TakePart(4))
        //     .TakeParts(1, 2, 3, 4, 5).Start();
        //
        // Dashboard.Create()
        //     .TakeParts(1, 2, 3).FlowFromSelf()
        //     .TakePart(1).FlowTo(x => x.TakeParts(2, 3, 4));
        //
        // Dashboard.Create()
        //     .TakeParts(1, 2, 3).Scale(100).FlowFromSelf()
        //     .TakePart(1).FlowTo(x => x.TakeParts(2, 3, 4));
        //
        // Dashboard.Create()
        //     .TakeParts(1, 2, 3).Scale(100).FlowFromSelf()
        //     .TakePart(1).FlowTo(x => x.TakeParts(2, 3, 4), "abcde");
        //
        // Dashboard.Create()
        //     .TakeParts(1, 2, 3).Scale(100).FlowFromSelf()
        //     .TakePart(1).FlowTo(x => x.TakeParts(2, 3, 4),
        //         "topic", "abcde");
        //
        // Dashboard.Create()
        //     .TakeParts(1, 2, 3).Scale(100).FlowFromSelf()
        //     .TakePart(1).FlowFrom(x => x.TakeParts(2, 3, 4),
        //         "queue", "abcde");
    }
}