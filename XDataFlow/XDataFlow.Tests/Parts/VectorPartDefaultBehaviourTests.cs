using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Behaviours;
using XDataFlow.Builders;
using XDataFlow.Context;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Tunnels.InMemory;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class VectorPartDefaultBehaviourTests
    {
        [Fact]
        public async Task VectorPartShouldSupportTryCatchStartBehaviour()
        {
            // Arrange
            var started = false;
            var finalized = false;
            var processedOk = false;
            var processedWithFailure = false;

            var part = PartDefaultBuilder.CreateVectorPart<VectorPartWithFailure,
                VectorPartWithFailure.Input,
                VectorPartWithFailure.Output>();

            part.ConfigureStartAs<TryCatchStart>(() => new TryCatchStart(
                () => started = true,
                () => processedOk = true,
                (ex) => processedWithFailure = true,
                () => finalized = true));

            part.SetupBindingAsTopicToQueue("testTopic", "testQueue", "testRoutingKey");

            // Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>
            {
                part.PushDataToFirstTunnel(new VectorPartWithFailure.Input() { });
                await part.StartAsync();
            });

            result.Message.Should().Be("Some Exception");

            // Assert
            started.Should().BeTrue();
            finalized.Should().BeTrue();
            processedWithFailure.Should().BeTrue();
            processedOk.Should().BeFalse();
        }
        
        [Fact]
        public async Task VectorPartShouldSupportStartInBackgroundBehaviour()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            var part = PartDefaultBuilder.CreateVectorPart<TestVectorPart,
                TestVectorPart.Input,
                TestVectorPart.Output>();

            part.ConfigureStartAs<StartInBackground>(() => new StartInBackground());
            part.SetupBindingAsTopicToQueue("testTopic", "testQueue", "testRoutingKey");

            // Act
            part.PushDataToFirstTunnel(new TestVectorPart.Input() { });
#pragma warning disable 4014
            Task.Run(async () =>
#pragma warning restore 4014
            {
                await Task.Delay(TimeSpan.FromSeconds(3), cts.Token);
                await part.StopAsync();
            }, cts.Token);
            
            await part.StartAsync();

            // Assert
            part.Should().Be("ABCDE");
        }
        
        private static IPartBuilder PartDefaultBuilder = new PartBuilder(
            (() => new Mock<IMetaDataContext>().Object),
            () => new Mock<IGroupContext>().Object,
            ()=> new Mock<IHeartBeatContext>().Object,
            ()=>new Mock<IConsumeMetrics>().Object);
        
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