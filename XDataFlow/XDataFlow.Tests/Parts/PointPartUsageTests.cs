using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Builders;
using XDataFlow.Controllers.Consume;
using XDataFlow.Controllers.Group;
using XDataFlow.Controllers.MetaData;
using XDataFlow.Controllers.Metric;
using XDataFlow.Controllers.Switch.Behaviours;
using XDataFlow.Parts.Abstractions;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class PointPartUsageTests
    {
        [Fact]
        public async Task PointPartShouldSupportStartBehaviour()
        {
            // Arrange
            var part = PartDefaultBuilder.CreatePointPart<TestPointPart>();
            
            part.ConfigureStartAs<Start>();

            // Act
            await part.StartAsync();

            // Assert
            part.TestProperty.Should().Be("ABCDE");
        }

        [Fact]
        public async Task PointPartShouldSupportStartInBackgroundBehaviour()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            var part = PartDefaultBuilder.CreatePointPart<TestPointPart>();

            var startInBackground = new StartInBackground(cts.Token);
            part.ConfigureStartAs(() => startInBackground);
            
            // Act
            await part.StartAsync();

            // Assert
            part.TestProperty.Should().Be("ABCDE");
        }
        
        private static IPartBuilder PartDefaultBuilder = new PartBuilder(
            (() => new Mock<IMetaDataController>().Object),
            () => new Mock<IGroupController>().Object,
            ()=> new Mock<IHeartBeatController>().Object,
            ()=>new Mock<IConsumeMetrics>().Object);

        private class TestPointPart : PointPart
        {
            public string TestProperty { get; set; }
            
            public override async Task ProcessAsync(CancellationToken cancellationToken)
            {
                await Task.Delay(100, cancellationToken);
                this.TestProperty = "ABCDE";
            }
        }
    }
}