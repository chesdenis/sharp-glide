using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SharpGlide.Context;
using SharpGlide.Context.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace SharpGlide.Tests.Unit.Context
{
    public class PublishContextTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public PublishContextTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        [Fact]
        public async Task ShouldSupportCustomPublishLogic()
        {
            // Arrange
            var testConsumeTunnel = new TestPublishTunnel();
            var heartBeatMock = new Mock<IVectorHeartBeatContext>();
            
            // Act
            var sut = new PublishContext<decimal>(heartBeatMock.Object);
            // sut.SetConsumeFlow(testConsumeTunnel.GetConsumeExpression());
            // var consumeData = sut.Consume();

            // Assert
            // consumeData.Should().HaveCount(1);
            // consumeData.First().Should().Be(1.234m);
        }
        
        public class TestPublishTunnel
        {
            public readonly List<decimal> PublishedData = new();
            public Expression<Action<IEnumerable<decimal>>> GetPublishExpression() => 
                (data) => Publish(data);

            public void Publish(IEnumerable<decimal> data)
            {
                PublishedData.AddRange(data);
            }
        }
    }
    
    public class ConsumeContextTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ConsumeContextTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ShouldSupportCustomConsumeLogic()
        {
            // Arrange
            var testConsumeTunnel = new TestConsumeTunnel();
            
            // Act
            var sut = new ConsumeContext<decimal>();
            sut.SetConsumeFlow(testConsumeTunnel.GetConsumeExpression());
            var consumeData = sut.Consume();

            // Assert
            consumeData.Should().HaveCount(1);
            consumeData.First().Should().Be(1.234m);
        }

        public class TestConsumeTunnel
        {
            public Expression<Func<IEnumerable<decimal>>> GetConsumeExpression() => () => Consume();

            public IEnumerable<decimal> Consume()
            {
                return new[] { 1.234m };
            }
        }
    }
}