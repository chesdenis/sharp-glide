using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Metric;
using XDataFlow.Refactored.Controllers.Switch.Behaviours;
using XDataFlow.Refactored.Parts;
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
            var partTemplate = PartDefaultBuilder.GetTemplate<TestPointPart>();
            var part = partTemplate();

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