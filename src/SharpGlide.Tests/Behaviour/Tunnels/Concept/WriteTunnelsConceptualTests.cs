using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Routing;
using SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept
{
    // public class WriteTunnelsConceptualTests
    // {
    //     [Fact]
    //     public async Task ShouldWriteDirectly()
    //     {
    //         // Arrange
    //         var sut = new DirectWriterExample();
    //         var publishLogic = sut.WriteExpr.Compile();
    //
    //         // Act
    //         publishLogic(10, Route.Default);
    //
    //         // Assert
    //         sut.Stack.Count.Should().Be(1);
    //         sut.Stack.ToList()[0].Should().Be(10);
    //     } 
    //     
    //     [Fact]
    //     public async Task ShouldWriteRangeDirectly()
    //     {
    //         // Arrange
    //         var sut = new DirectWriterExample();
    //         var publishRangeLogic = sut.WriteRangeExpr.Compile();
    //
    //         // Act
    //         publishRangeLogic(new decimal[]{10, 20, 30}, Route.Default);
    //
    //         // Assert
    //         sut.Stack.Count.Should().Be(3);
    //         sut.Stack.ToList()[0].Should().Be(30);
    //         sut.Stack.ToList()[1].Should().Be(20);
    //         sut.Stack.ToList()[2].Should().Be(10);
    //     }
    // }
}