using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Behaviours;
using SharpGlide.Tests.Model;
using FluentAssertions;
using Xunit;

namespace SharpGlide.Tests.Parts
{
    public class PointPartDefaultBehavioursTests
    {
        [Fact]
        public async Task PointPartShouldSupportTryCatchStartBehaviour()
        {
            // Arrange
            var started = false;
            var finalized = false;
            var processedOk = false;
            var processedWithFailure = false;
            var part = new TestPointPartWithFailure();
            part.ConfigureStartAs(() => new TryCatchStart(
                () => started = true,
                () => processedOk = true,
                ex => processedWithFailure = true,
                () => finalized = true));
            // Act
            await part.StartAsync();

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
    }
}