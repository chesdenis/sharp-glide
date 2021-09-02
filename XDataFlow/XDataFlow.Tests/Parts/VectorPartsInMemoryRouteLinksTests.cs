using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Behaviours;
using XDataFlow.Context;
using XDataFlow.Extensions;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;
using XDataFlow.Tests.Model;
using XDataFlow.Tunnels.InMemory.Messaging;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class VectorPartsInMemoryRouteLinksTests
    {
        [Fact]
        public async Task VectorPartShouldFlowDataToAnotherVectorPart()
        {
            // Arrange
            var partA = new SimplePublisherWithTwoRoutingWay();
            var partB = new SimpleReceiverOfAFlow();
            var partC = new SimpleReceiverOfBFlow();
            
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();
            partC.ConfigureStartAs<StartInBackground>();
            
            partA.FlowFromSelf();
            partA.FlowTo(partB, "Rk.AFlow");
            partA.FlowTo(partC, "Rk.BFlow");

            // Act
            partA.ConsumeData(10);
            await partA.StartAsync();
            await partB.StartAsync();
            await partC.StartAsync();
            
            // Assert
            partA.WasPublished("20").Should().BeTrue();
            partA.WasPublished("30").Should().BeTrue();
            
            partB.WasConsumed("20").Should().BeTrue();
            partB.WasConsumed("30").Should().BeFalse();
            
            partC.WasConsumed("30").Should().BeTrue();
            partC.WasConsumed("20").Should().BeFalse();
        }
        
        public class SimplePublisherWithTwoRoutingWay : AssertableVectorPart<int, string>
        {
            public override async Task ProcessAsync(int data, CancellationToken cancellationToken)
            {
                this.ConsumedData.Add(data);
                
                var valueA = data * 2;
                var valueB = data * 3;
                
                this.Publish(Map(valueA), "Rk.AFlow");
                this.PublishedData.Add(Map(valueA));
                this.Publish(Map(valueB), "Rk.BFlow");
                this.PublishedData.Add(Map(valueB));

                await this.StopAsync();
            }

            public override string Map(int data) => data.ToString();
        }
        
        public class SimpleReceiverOfAFlow : AssertableVectorPart<string, byte[]>
        {
            public override async Task ProcessAsync(string data, CancellationToken cancellationToken)
            {
                this.ConsumedData.Add(data);

                await this.StopAsync();
            }

            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }
        
        public class SimpleReceiverOfBFlow : AssertableVectorPart<string, byte[]>
        {
            public override async Task ProcessAsync(string data, CancellationToken cancellationToken)
            {
                this.ConsumedData.Add(data);
                await this.StopAsync();
            }

            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }
    }
}