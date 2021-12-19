using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Behaviours;
using SharpGlide.Tests.Model.VectorPart;
using SharpGlide.Tunnels.InMemory;
using SharpGlide.Tunnels.InMemory.Messaging;
using SharpGlide.Tunnels.Routes;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
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
                new ConsumeRoute
                {
                    Topic = "t1",
                    Queue = "q1",
                    RoutingKey = "r1"
                });
            partStable.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPart.Input>(InMemoryBroker.Current),
                new ConsumeRoute()
                {
                    Topic = "t2", 
                    Queue = "q2", 
                    RoutingKey = "r2"
                });


            // Act
            partWithFailure.TakeAndConsume(new TestVectorPartWithFailure.Input());
            await partWithFailure.StartAsync();

            partStable.TakeAndConsume(new TestVectorPart.Input());
            await partStable.StartAndStopAsync(TimeSpan.FromSeconds(1));
 
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
                new ConsumeRoute()
                {
                    Topic = "t1",
                    Queue = "q1",
                    RoutingKey = "r1"
                });
            partStable.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPart.Input>(InMemoryBroker.Current),
                new ConsumeRoute()
                {
                    Topic = "t2",
                    Queue = "q2",
                    RoutingKey = "r2"
                });


            // Act
            partWithFailure.TakeAndConsume(new TestVectorPartWithFailure.Input());
            await partWithFailure.StartAsync();

            partStable.TakeAndConsume(new TestVectorPart.Input());
            await partStable.StartAndStopAsync(TimeSpan.FromSeconds(1));

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
                ConsumeRoute.Default);

            // Act
            part.TakeAndConsume(new TestVectorPart.Input());
            await part.StartAndStopAsync(TimeSpan.FromSeconds(1));

            // Assert
            part.TestProperty.Should().Be("ABCDE");
        }
    }
}