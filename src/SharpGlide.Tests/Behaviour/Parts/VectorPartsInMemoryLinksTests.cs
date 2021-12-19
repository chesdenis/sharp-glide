using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Behaviours;
using SharpGlide.Extensions;
using SharpGlide.Tests.Model.VectorPart;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
{
    public class VectorPartsInMemoryLinksTests
    {
        [Fact]
        public async Task VectorPartShouldFlowDataToAnotherVectorPart()
        {
            // Arrange
            var partA = new TestVectorA();
            var partB = new TestVectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();
            
            partA.FlowFromSelf().FlowTo(partB);

            // Act
            partA.TakeAndConsume(123);
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
            var partA = new TestVectorA();
            var partB = new TestVectorB();
            var partC = new TestVectorC();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();
            partC.ConfigureStartAs<StartInBackground>();
            
            partA.FlowFromSelf().FlowTo(partB);
            partA.FlowTo(partC);
            
            // Act
            partA.TakeAndConsume(123);
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
            var partA = new TestVectorA();
            var partB = new TestVectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();
            
            partA.FlowFromSelf().FlowTo(partB);

            // Act
            partA.TakeAndConsume(123);
            await partA.StartAsync();
            await partB.StartAsync();

            // Assert
            partA.WasPublished("123").Should().BeTrue();
            partB.WasConsumed("123").Should().BeTrue();
        }

        public class TestVectorA : TestVectorPartAssertableDirect<int, string>
        {
            public override string Map(int data) => data.ToString();
        }
        
        public class TestVectorB : TestVectorPartAssertableDirect<string, byte[]>
        {
            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }
        
        public class TestVectorC: TestVectorPartAssertableDirect<string, byte[]>
        {
            public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        }
        
        public class TestVectorD:TestVectorPartAssertableDirect<byte[], int>
        {
            public override int Map(byte[] data) => Convert.ToInt32(Encoding.UTF8.GetString(data));
        }
    }
}