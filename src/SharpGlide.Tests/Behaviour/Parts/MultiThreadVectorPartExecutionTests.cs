using System;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Extensions;
using SharpGlide.Flows;
using SharpGlide.Tests.Model.VectorPart;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
{
    public class MultiThreadVectorPartExecutionTests
    {
        // [Fact]
        // public async Task ShouldCreateMultipleThreadsOnceReceiveMessages()
        // {
        //     // Arrange
        //     var multiThreadVectorPart = new TestVectorPartMultiThreadWithExecutionJob();
        //     multiThreadVectorPart.FlowFromSelf();
        //
        //     // Act
        //     multiThreadVectorPart.TakeAndConsumeRange(new int[] { 1, 2, 3, 4, 5, 6 });
        //     await multiThreadVectorPart.StartAndStopAsync(TimeSpan.FromSeconds(3));
        //
        //     // Assert
        //     multiThreadVectorPart.PushedData.Count.Should().Be(6);
        // }
    }
}