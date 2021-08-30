using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Behaviours;
using XDataFlow.Builders;
using XDataFlow.Context;
using XDataFlow.Extensions;
using XDataFlow.Parts.Abstractions;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class VectorPartsInMemoryLinksTests
    {
        public VectorPartsInMemoryLinksTests()
        {
            SetupDefaults();
        }
        
        [Fact]
        public async Task VectorPartShouldCreateSimpleOneWayLinkToAnotherVectorPart()
        {
            // Arrange
            var partA = new VectorA();
            var partB = new VectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();

            partA.FlowFromSelf();
            partA.FlowTo(partB);
            // partA.PublishTo<CustomBroker>(partB);
            // partA.PublishTo(partB, "t1", "q1");
            // partA.PublishTo(partB, "t2", "q2", "rk1", "rk2");
            
            // var partC = VectorC.CreateUsing(MockPartBuilder);
            // var partD = VectorD.CreateUsing(MockPartBuilder);

            // partD.ConsumeFrom(partC);
            // partD.ConsumeFrom<CustomBroker>(partC);
            // partD.ConsumeFrom(partC, "t1", "q1");
            // partD.ConsumeFrom(partC, "t2", "q2", "rk1", "rk2");
            
            // Act
            partA.ConsumeData(123);
            await partA.StartAndStopAsync(TimeSpan.FromSeconds(2));
            await partB.StartAndStopAsync(TimeSpan.FromSeconds(2));

            // Assert
            partA.WasPublished("123").Should().BeTrue();
            partB.WasConsumed("123").Should().BeTrue();
        }

        private static void SetupDefaults()
        {
            XFlowDefault.Set<IMetaDataContext>(() => new Mock<IMetaDataContext>().Object);
            XFlowDefault.Set<IGroupContext>(()=>new Mock<IGroupContext>().Object);
            XFlowDefault.Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            XFlowDefault.Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
        }
        
        public abstract class AssertableVectorPart<TConsumeData, TPublishData> : VectorPart<TConsumeData, TPublishData>
        {
            public List<TConsumeData> ConsumedData { get; } = new();
            public List<TPublishData> PublishedData { get; } = new();

            public bool WasConsumed(TConsumeData data)
            {
                return Enumerable.Contains(ConsumedData, data);
            }

            public bool WasPublished(TPublishData data)
            {
                return Enumerable.Contains(PublishedData, data);
            }
            
            public override Task ProcessAsync(TConsumeData data, CancellationToken cancellationToken)
            {
                var publishData = Map(data);
                
                this.Publish(publishData);
                
                this.ConsumedData.Add(data);
                this.PublishedData.Add(publishData);
                
                return Task.CompletedTask;
            }

            public abstract TPublishData Map(TConsumeData data);
        }
        
        public class VectorA : AssertableVectorPart<int, string>
        {
            public override string Map(int data) => data.ToString();
        }
        
        public class VectorB : AssertableVectorPart<string, byte[]>
        {
            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }
        
        public class VectorC: AssertableVectorPart<string, byte[]>
        {
            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }
        
        public class VectorD:AssertableVectorPart<byte[], int>
        {
            public override int Map(byte[] data) => Convert.ToInt32(Encoding.UTF8.GetString(data));
        }
    }
}