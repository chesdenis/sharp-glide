using System;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Extensions;
using SharpGlide.Tests.Model;
using Xunit;

namespace SharpGlide.Tests.Parts
{
    public class MultiThreadVectorPartExecutionTests
    {
        [Fact]
        public async Task ShouldCreateMultipleThreadsOnceReceiveMessages()
        {
            // Arrange
            var multiThreadVectorPart = new TestMultiThreadVectorPartWithExecutionJob();
            multiThreadVectorPart.FlowFromSelf();

            // Act
            multiThreadVectorPart.PushRange(new int[] { 1, 2, 3, 4, 5, 6 });
            await multiThreadVectorPart.StartAndStopAsync(TimeSpan.FromSeconds(3));

            // Assert
            multiThreadVectorPart.PushedData.Count.Should().Be(6);
        }
      
    }
}