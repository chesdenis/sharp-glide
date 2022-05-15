using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SharpGlide.Processing.Tests
{
    public class CalculateProcessingSpeedVectorPartTests
    {
        [Fact]
        public async Task ShouldSupportGeneralCalculation()
        {
            // Arrange
            var flowMetrics = new SpeedMeasurePart.Metric[]
            {
                new() { Current = 10, Total = 100, ElapsedMs = 100 },
                new() { Current = 20, Total = 100, ElapsedMs = 200 },
                new() { Current = 25, Total = 100, ElapsedMs = 300 },
                new() { Current = 40, Total = 100, ElapsedMs = 400 },
                new() { Current = 50, Total = 100, ElapsedMs = 500 },
            };

            // Act
            var sut = new SpeedMeasurePart();
            var outputData = new List<SpeedMeasurePart.TimeAndSpeed>();

            foreach (var flowMetric in flowMetrics)
            {
                var data = await sut.TransformAsync(flowMetric, CancellationToken.None);
                outputData.Add(data);
            }

            // Assert
            outputData.Count.Should().Be(flowMetrics.Length);

            Math.Round(outputData[2].Progress, 2).Should().Be(25);
            Math.Round(outputData[2].Current, 2).Should().Be(25);
            Math.Round(outputData[2].Total, 2).Should().Be(100);

            outputData[2].TimeSpent.TotalMilliseconds.Should().Be(300);
            Math.Round(outputData[2].SpeedMs, 3).Should().Be(0.183);
            Math.Round(outputData[2].SpeedSec, 3).Should().Be(183.333);
            Math.Round(outputData[4].FinishIn.TotalMilliseconds, 2).Should().Be(172.41);
        }
    }
}