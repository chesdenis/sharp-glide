using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Behaviours;
using XDataFlow.Context;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;
using XDataFlow.Tunnels.InMemory;
using XDataFlow.Tunnels.InMemory.Messaging;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class VectorPartDefaultBehaviourTests
    {
        public VectorPartDefaultBehaviourTests()
        {
            SetupDefaults();
        }
        
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

            var partWithFailure = new VectorPartWithFailure();
            var partStable = new TestVectorPart();

            partWithFailure.ConfigureStartAs<TryCatchStart>(() => new TryCatchStartInBackground(
                () => partWithFailureStarted = true,
                () => partWithFailureProcessedOk = true,
                (ex) => partWithFailureProcessedFailed = true,
                () => partWithFailureFinalized = true));
            partStable.ConfigureStartAs<TryCatchStart>(() => new TryCatchStartInBackground(
                () => partStableStarted = true,
                () => partStableProcessedOk = true,
                (ex) => partStableProcessedFailed = true,
                () => partStableFinalized = true));

            partWithFailure.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<VectorPartWithFailure.Input>(InMemoryBroker.Current),
                "t1", "q1", "r1");
            partStable.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPart.Input>(InMemoryBroker.Current),
                "t2", "q2", "r2");


            // Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>
            {
                partWithFailure.ConsumeData(new VectorPartWithFailure.Input() { });
                await partWithFailure.StartAsync();
            });

            partStable.ConsumeData(new TestVectorPart.Input() { });
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

            var partWithFailure = new VectorPartWithFailure();
            var partStable = new TestVectorPart();

            partWithFailure.ConfigureStartAs<TryCatchStart>(() => new TryCatchStart(
                () => partWithFailureStarted = true,
                () => partWithFailureProcessedOk = true,
                (ex) => partWithFailureProcessedFailed = true,
                () => partWithFailureFinalized = true));
            partStable.ConfigureStartAs<TryCatchStart>(() => new TryCatchStart(
                () => partStableStarted = true,
                () => partStableProcessedOk = true,
                (ex) => partStableProcessedFailed = true,
                () => partStableFinalized = true));

            partWithFailure.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<VectorPartWithFailure.Input>(InMemoryBroker.Current),
                "t1", "q1", "r1");
            partStable.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPart.Input>(InMemoryBroker.Current),
                "t2", "q2", "r2");


            // Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>
            {
                partWithFailure.ConsumeData(new VectorPartWithFailure.Input() { });
                await partWithFailure.StartAsync();
            });

            partStable.ConsumeData(new TestVectorPart.Input() { });
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

            part.ConfigureStartAs<StartInBackground>(() => new StartInBackground());
            
            part.SetupConsumeAsQueueFromTopic(
                new InMemoryConsumeTunnel<TestVectorPart.Input>(InMemoryBroker.Current),
                "t2", "q2", "r2");

            // Act
            part.ConsumeData(new TestVectorPart.Input() { });
            await part.StartAndStopAsync(TimeSpan.FromSeconds(1));

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

        private class TestVectorPart : VectorPart<TestVectorPart.Input, TestVectorPart.Output>
        {
            public class Input
            {
                
            }
            
            public class Output
            {
                
            }
            
            public string TestProperty { get; set; }

            public override async Task ProcessAsync(Input data, CancellationToken cancellationToken)
            {
                await Task.Delay(100, cancellationToken);
                this.TestProperty = "ABCDE";

                await this.StopAsync();
            }
        }
        
        private class VectorPartWithFailure : VectorPart<VectorPartWithFailure.Input, VectorPartWithFailure.Output>
        {
            public class Input
            {
                
            }
            
            public class Output
            {
                
            }
             
            public override async Task ProcessAsync(Input data, CancellationToken cancellationToken)
            {
                await Task.Delay(100, cancellationToken);

                throw new Exception("Some Exception");
            }
        }
    }
}