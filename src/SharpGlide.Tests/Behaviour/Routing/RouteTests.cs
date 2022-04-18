using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Routing;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Routing
{
    public class RouteTests
    {
        [Fact]
        public async Task ShouldDetectDifferentRoutes()
        {
            // Arrange
            var routeA = new Route
            {
                RoutingKey = "rk1",
                Queue = "q",
                Topic = "t"
            };
            var routeB = new Route() { };

            // Act
            var result = routeA.Equals(routeB);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldDetectSameRoutes()
        {
            // Arrange
            var routeA = new Route
            {
                RoutingKey = "rk1",
                Queue = "q",
                Topic = "t"
            };
            var routeB = new Route()
            {
                RoutingKey = "rk1",
                Queue = "q",
                Topic = "t"
            };

            // Act
            var result = routeA.Equals(routeB);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldDistinctOperatorWorkCorrectly()
        {
            // Arrange
            var routeA = new Route
            {
                RoutingKey = "rk1",
                Queue = "q",
                Topic = "t"
            };
            var routeB = new Route()
            {
                RoutingKey = "rk1",
                Queue = "q",
                Topic = "t"
            };

            // Act
            var distinct = new List<IRoute>() { routeA, routeB }.Distinct().ToArray();

            // Assert
            distinct.Should().HaveCount(1);
        }
    }
}