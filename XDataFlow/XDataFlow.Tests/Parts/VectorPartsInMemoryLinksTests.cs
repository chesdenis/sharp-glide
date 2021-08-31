using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using XDataFlow.Behaviours;
using XDataFlow.Context;
using XDataFlow.Extensions;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;
using XDataFlow.Tests.Model;
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
        public async Task VectorPartShouldFlowDataToAnotherVectorPart()
        {
            // Arrange
            var partA = new VectorA();
            var partB = new VectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();

            partA.FlowFromSelf();
            partA.FlowTo(partB);
            
            // Act
            partA.ConsumeData(123);
            await partA.StartAndStopAsync(TimeSpan.FromSeconds(2));
            await partB.StartAndStopAsync(TimeSpan.FromSeconds(2));

            // Assert
            partA.WasPublished("123").Should().BeTrue();
            partB.WasConsumed("123").Should().BeTrue();
        }
        
        [Fact]
        public async Task VectorPartShouldFlowDataToMultipleVectorParts()
        {
            // Arrange
            var partA = new VectorA();
            var partB = new VectorB();
            var partC = new VectorC();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();
            partC.ConfigureStartAs<StartInBackground>();

            partA.FlowFromSelf();
            partA.FlowTo(partB);
            partA.FlowTo(partC);
            
            // Act
            partA.ConsumeData(123);
            await partA.StartAndStopAsync(TimeSpan.FromSeconds(2));
            await partB.StartAndStopAsync(TimeSpan.FromSeconds(2));
            await partC.StartAndStopAsync(TimeSpan.FromSeconds(2));

            // Assert
            partA.WasPublished("123").Should().BeTrue();
            partB.WasConsumed("123").Should().BeTrue();
            partC.WasConsumed("123").Should().BeTrue();
        }
        
        [Fact]
        public async Task VectorPartShouldFlowDataFromAnotherVectorPart()
        {
            // Arrange
            var partA = new VectorA();
            var partB = new VectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();

            partA.FlowFromSelf();
            partB.FlowFrom(partA);
            
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
            XFlowDefaultRegistry.Set<IMetaDataContext>(() => new Mock<IMetaDataContext>().Object);
            XFlowDefaultRegistry.Set<IGroupContext>(()=>new Mock<IGroupContext>().Object);
            XFlowDefaultRegistry.Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            XFlowDefaultRegistry.Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
        }
        
        public class VectorA : DirectAssertableVectorPart<int, string>
        {
            public override string Map(int data) => data.ToString();
        }
        
        public class VectorB : DirectAssertableVectorPart<string, byte[]>
        {
            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }
        
        public class VectorC: DirectAssertableVectorPart<string, byte[]>
        {
            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }
        
        public class VectorD:DirectAssertableVectorPart<byte[], int>
        {
            public override int Map(byte[] data) => Convert.ToInt32(Encoding.UTF8.GetString(data));
        }
    }
}