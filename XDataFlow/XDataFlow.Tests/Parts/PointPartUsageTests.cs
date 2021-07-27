using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Parts.Interfaces;
using XDataFlow.Refactored;
using XDataFlow.Refactored.Behaviours;
using XDataFlow.Refactored.Builders;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class PointPartUsageTests
    {
        [Fact]
        public async Task PointPartShouldSupportStartBehaviour()
        {
            // Arrange
            var partTemplate = PartDefaultBuilder.GetTemplate<TestPointPart>();
            var part = partTemplate();
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
        
        private static PointPartBuilder PartDefaultBuilder = new PointPartBuilder(
            (() => new Mock<IMetaDataController>().Object),
            () => new Mock<IGroupController>().Object);

        private class TestPointPart : PointPart
        {
            public string TestProperty { get; set; }

            public TestPointPart(
                IMetaDataController metaDataController,
                IGroupController groupController) : base(metaDataController, groupController)
            {
            }
            
            public override async Task ProcessAsync(CancellationToken cancellationToken)
            {
                await Task.Delay(100, cancellationToken);
                this.TestProperty = "ABCDE";
            }
        }
    }
}