using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Behaviours;
using SharpGlide.Tests.Model;
using SharpGlide.Tunnels.InMemory;
using SharpGlide.Tunnels.InMemory.Messaging;
using FluentAssertions;
using Xunit;

namespace SharpGlide.Tests.Parts
{
    public class VectorPartDefaultBehaviourTests
    {
        [Fact]
        public async Task VectorPartShouldSupportTryCatchStartInBackgroundBehaviour()
        {
            // Arrange
            var partWithFailureStarted = false;
            var partWithFailureFinalized = false;
            var partWithFailureProcessedOk = false;
            var partWithFailureProcessedFailed = false;
            
            var partStableStarted = false;
            var partStableFinalized = false;
            var partStableProcessedOk = false;
            var partStableProcessedFailed = false;

            var partWithFailure = new TestVectorPartWithFailure();
            var partStable = new TestVectorPart();

            partWithFailure.ConfigureStartAs<TryCatchStart>(() => new TryCatchStartInBackground(
                () => partWithFailureStarted = true,
                () => partWithFailureProcessedOk = true,
                ex => partWithFailureProcessedFailed = true,
                () => partWithFailureFinalized = true));
            partStable.ConfigureStartAs<TryCatchStart>(() => new TryCatchStartInBackground(
                () => partStableStarted = true,
                () => partStableProcessedOk = true,
                ex => partStableProcessedFailed = true,
                () => partStableFinalized = true));

            partWithFailure.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPartWithFailure.Input>(InMemoryBroker.Current),
                "t1", "q1", "r1");
            partStable.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPart.Input>(InMemoryBroker.Current),
                "t2", "q2", "r2");


            // Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>
            {
                partWithFailure.Push(new TestVectorPartWithFailure.Input());
                await partWithFailure.StartAsync();
            });

            partStable.Push(new TestVectorPart.Input());
            await partStable.StartAndStopAsync(TimeSpan.FromSeconds(1));

            result.Message.Should().Be("Some Exception");

            // Assert
            partWithFailureStarted.Should().BeTrue();
            partWithFailureFinalized.Should().BeTrue();
            partWithFailureProcessedFailed.Should().BeTrue();
            partWithFailureProcessedOk.Should().BeFalse();
            
            partStableStarted.Should().BeTrue();
            partStableFinalized.Should().BeTrue();
            partStableProcessedFailed.Should().BeFalse();
            partStableProcessedOk.Should().BeTrue();
        }
        
        [Fact]
        public async Task VectorPartShouldSupportTryCatchStartBehaviour()
        {
            // Arrange
            var partWithFailureStarted = false;
            var partWithFailureFinalized = false;
            var partWithFailureProcessedOk = false;
            var partWithFailureProcessedFailed = false;
            
            var partStableStarted = false;
            var partStableFinalized = false;
            var partStableProcessedOk = false;
            var partStableProcessedFailed = false;

            var partWithFailure = new TestVectorPartWithFailure();
            var partStable = new TestVectorPart();

            partWithFailure.ConfigureStartAs(() => new TryCatchStart(
                () => partWithFailureStarted = true,
                () => partWithFailureProcessedOk = true,
                ex => partWithFailureProcessedFailed = true,
                () => partWithFailureFinalized = true));
            partStable.ConfigureStartAs(() => new TryCatchStart(
                () => partStableStarted = true,
                () => partStableProcessedOk = true,
                ex => partStableProcessedFailed = true,
                () => partStableFinalized = true));

            partWithFailure.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPartWithFailure.Input>(InMemoryBroker.Current),
                "t1", "q1", "r1");
            partStable.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPart.Input>(InMemoryBroker.Current),
                "t2", "q2", "r2");


            // Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>
            {
                partWithFailure.Push(new TestVectorPartWithFailure.Input());
                await partWithFailure.StartAsync();
            });

            partStable.Push(new TestVectorPart.Input());
            await partStable.StartAndStopAsync(TimeSpan.FromSeconds(1));

            result.Message.Should().Be("Some Exception");

            // Assert
            partWithFailureStarted.Should().BeTrue();
            partWithFailureFinalized.Should().BeTrue();
            partWithFailureProcessedFailed.Should().BeTrue();
            partWithFailureProcessedOk.Should().BeFalse();
            
            partStableStarted.Should().BeTrue();
            partStableFinalized.Should().BeTrue();
            partStableProcessedFailed.Should().BeFalse();
            partStableProcessedOk.Should().BeTrue();
        }
        
        [Fact]
        public async Task VectorPartShouldSupportStartInBackgroundBehaviour()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            var part = new TestVectorPart();

            part.ConfigureStartAs(() => new StartInBackground());
            
            part.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPart.Input>(InMemoryBroker.Current),
                "t2", "q2", "r2");

            // Act
            part.Push(new TestVectorPart.Input());
            await part.StartAndStopAsync(TimeSpan.FromSeconds(1));

            // Assert
            part.TestProperty.Should().Be("ABCDE");
        }
    }
}