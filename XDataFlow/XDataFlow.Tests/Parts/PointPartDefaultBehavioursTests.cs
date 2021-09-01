using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Behaviours;
using XDataFlow.Context;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Providers;
using XDataFlow.Registry;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class PointPartDefaultBehavioursTests
    {
        public PointPartDefaultBehavioursTests()
        {
            SetupDefaults();
        }
        
        [Fact]
        public async Task PointPartShouldSupportTryCatchStartBehaviour()
        {
            // Arrange
            var started = false;
            var finalized = false;
            var processedOk = false;
            var processedWithFailure = false;
            var part = new PointPartWithFailure();
            part.ConfigureStartAs<TryCatchStart>(() => new TryCatchStart(
                () => started = true,
                () => processedOk = true,
                (ex) => processedWithFailure = true,
                () => finalized = true));
            // Act
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await part.StartAsync();
            });
            
            // Assert
            started.Should().BeTrue();
            finalized.Should().BeTrue();
            processedWithFailure.Should().BeTrue();
            processedOk.Should().BeFalse();
        }

        [Fact]
        public async Task PointPartShouldSupportStartBehaviour()
        {
            // Arrange
            var part = new TestPointPart();
            
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
            var part = new TestPointPart();

            var startInBackground = new StartInBackground();
            part.ConfigureStartAs(() => startInBackground);
            
            // Act
            await part.StartAsync();

            // Assert
            part.TestProperty.Should().Be("ABCDE");
        }

        private static void SetupDefaults()
        {
            XFlowDefaultRegistry.Set<IMetaDataContext>(() => new Mock<IMetaDataContext>().Object);
            XFlowDefaultRegistry.Set<IGroupContext>(()=>new Mock<IGroupContext>().Object);
            XFlowDefaultRegistry.Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            XFlowDefaultRegistry.Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            XFlowDefaultRegistry.Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
        }

        private class TestPointPart : PointPart
        {
            public string TestProperty { get; set; }
            
            public override async Task ProcessAsync(CancellationToken cancellationToken)
            {
                await Task.Delay(100, cancellationToken);
                this.TestProperty = "ABCDE";

                await this.StopAsync();
            }
        }
        
        private class PointPartWithFailure : PointPart
        {
            public override async Task ProcessAsync(CancellationToken cancellationToken)
            {
                await Task.Delay(100, cancellationToken);

                throw new Exception("Some Exception");
            }
        }
    }
}