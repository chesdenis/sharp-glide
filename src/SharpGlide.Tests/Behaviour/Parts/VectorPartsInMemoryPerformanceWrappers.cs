using System;
using System.Text;
using System.Threading.Tasks;
using SharpGlide.Behaviours;
using SharpGlide.Extensions;
using SharpGlide.Tests.Model.VectorPart;
using SharpGlide.TunnelWrappers.Performance;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
{
    public class VectorPartsInMemoryPerformanceWrappers
    {
        [Fact]
        public async Task adsasd()
        {
            // Arrange
            var partA = new TestVectorA();
            var partB = new TestVectorB();
            partA.ConfigureStartAs<StartInBackground>();
            partB.ConfigureStartAs<StartInBackground>();
            
            partA.FlowFromSelf(x=>x.AddOutputWrapper(() => new MeasureConsumeSpeedConsumeWrapper<int>()))
                .FlowTo(partB);
            
            // Act
            
            // Assert
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