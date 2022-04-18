using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Writers;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Tunnels
{
    public class WriteTunnelsConceptualTests
    {
        [Fact]
        public async Task ShouldWriteDirectly()
        {
            // Arrange
            var sut = new WriteTunnelExample();
            var publishLogic = sut.WriteExpr.Compile();

            // Act
            publishLogic(10, Route.Default);

            // Assert
            sut.Stack.Count.Should().Be(1);
            sut.Stack.ToList()[0].Should().Be(10);
        } 
        
        [Fact]
        public async Task ShouldWriteRangeDirectly()
        {
            // Arrange
            var sut = new WriteTunnelExample();
            var publishRangeLogic = sut.WriteRangeExpr.Compile();

            // Act
            publishRangeLogic(new decimal[]{10, 20, 30}, Route.Default);

            // Assert
            sut.Stack.Count.Should().Be(3);
            sut.Stack.ToList()[0].Should().Be(30);
            sut.Stack.ToList()[1].Should().Be(20);
            sut.Stack.ToList()[2].Should().Be(10);
        }
        
        private class WriteTunnelExample : IWriteDirectlyTunnel<decimal>
        {
            public readonly ConcurrentStack<decimal> Stack = new();

            public bool CanExecute { get; set; } = true;
            public Expression<Action<decimal, IRoute>> WriteExpr
                => (v, rt) => WriteLogic(v,rt);

            public Expression<Task<Action<IEnumerable<decimal>, IRoute>>> WriteRangeAsyncExpr { get; }

            private void WriteLogic(decimal v, IRoute rt)
            {
                Stack.Push(v);
            }
            
            private void WriteRangeLogic(IEnumerable<decimal> vs, IRoute rt)
            {
               Stack.PushRange(vs.ToArray());
            }

            public Expression<Action<IEnumerable<decimal>, IRoute>> WriteRangeExpr =>
                (vs, rt) => WriteRangeLogic(vs, rt);
        }
    }
}