using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Context;
using Xunit;
using Xunit.Abstractions;

namespace SharpGlide.Tests.Unit.Context
{
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
            sut.BuildConsumeLogic(testConsumeTunnel.GetConsumeExpression());
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