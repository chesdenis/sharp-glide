using System;
using System.Text;
using System.Threading.Tasks;
using SharpGlide.Behaviours;
using SharpGlide.Extensions;
using SharpGlide.Tests.Model;
using FluentAssertions;
using Xunit;

namespace SharpGlide.Tests.Parts
{
    public class VectorPartsInMemoryLinksTests
    {
        [Fact]
        public async Task VectorPartShouldFlowDataToAnotherVectorPart()
        {
            // Arrange
            var partA = new VectorA();
            var partB = new VectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();
            
            partA.FlowFromSelf().FlowTo(partB);

            // Act
            partA.Push(123);
            await partA.StartAsync();
            await partB.StartAsync();

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
            
            partA.FlowFromSelf().FlowTo(partB);
            partA.FlowTo(partC);
            
            // Act
            partA.Push(123);
            await partA.StartAsync();
            await partB.StartAsync();
            await partC.StartAsync();

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
            
            partA.FlowFromSelf().FlowTo(partB);

            // Act
            partA.Push(123);
            await partA.StartAsync();
            await partB.StartAsync();

            // Assert
            partA.WasPublished("123").Should().BeTrue();
            partB.WasConsumed("123").Should().BeTrue();
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