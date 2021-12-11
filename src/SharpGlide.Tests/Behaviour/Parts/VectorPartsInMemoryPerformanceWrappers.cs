using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Behaviours;
using SharpGlide.Extensions;
using SharpGlide.Tests.Model.VectorPart;
using SharpGlide.TunnelWrappers.Extensions;
using SharpGlide.TunnelWrappers.Performance;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
{
    public class VectorPartsInMemoryPerformanceWrappers
    {
        [Fact]
        public async Task ShouldRecordConsumeSpeed()
        {
            // Arrange
            var partA = new TestVectorA();
            var partB = new TestVectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();

            partA.FlowFromSelf(
                x =>
                    x.AddConsumeWrapper<int, MeasureConsumeSpeedConsumeWrapper<int>>());

            // Act
            partA.Consume(10);
            await partA.StartAsync();

            // Assert
            partA.WasPublished("10").Should().BeTrue();

            var wrapper = partA
                .GetConsumeWrapper<int, MeasureConsumeSpeedConsumeWrapper<int>>().First();

            var speedMetric = wrapper.GetMetric();
            speedMetric.Count.Should().BePositive();
        }

        [Fact]
        public async Task ShouldRecordConsumeAndPublishSpeed()
        {
            // Arrange
            var partA = new TestVectorA();
            var partB = new TestVectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();

            var partFlow = partA.FlowFromSelf(
                x =>
                    x.AddConsumeWrapper<int, MeasureConsumeSpeedConsumeWrapper<int>>());

            // because partA publish string data and partB consume string data - both wrappers are string based
            partFlow.FlowTo(partB,
                x =>
                    x.AddConsumeWrapper<string, MeasureConsumeSpeedConsumeWrapper<string>>(),
                x => x.AddPublishWrapper<string, MeasurePublishSpeedPublishWrapper<string>>());

            // Act
            partA.Consume(10);
            await partA.StartAsync();
            await partB.StartAsync();

            // Assert
            partA.WasPublished("10").Should().BeTrue();
            partB.WasConsumed("10").Should().BeTrue();

            var partAFlowInWrapper = partA
                .GetConsumeWrapper<int, MeasureConsumeSpeedConsumeWrapper<int>>().First();

            var partAFlowOutWrapper =
                partA.GetPublishWrapper<string, MeasurePublishSpeedPublishWrapper<string>>().First();

            var partAFlowInMetric = partAFlowInWrapper.GetMetric();
            partAFlowInMetric.Count.Should().BePositive();   
            
            var partAFlowOutMetric = partAFlowOutWrapper.GetMetric();
            partAFlowOutMetric.Count.Should().BePositive();
        }

        public class TestVectorA : TestVectorPartAssertableDirect<int, string>
        {
            public override string Map(int data) => data.ToString();
        }

        public class TestVectorB : TestVectorPartAssertableDirect<string, byte[]>
        {
            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }

        public class TestVectorC : TestVectorPartAssertableDirect<string, byte[]>
        {
            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }

        public class TestVectorD : TestVectorPartAssertableDirect<byte[], int>
        {
            public override int Map(byte[] data) => Convert.ToInt32(Encoding.UTF8.GetString(data));
        }
    }
}