using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Exceptions;
using SharpGlide.Tunnels.Readers;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Tunnels
{
    public class ReadTunnelsConceptualTests
    {
        [Fact]
        public async Task ShouldConsumeSingleMessage()
        {
            // Arrange
            var sut = new ReadTunnelExample();
            var consumeFunc = sut.ReadExpr.Compile();

            // Act
            sut.Stack.Push(123);
            var data = consumeFunc();

            // Assert
            data.Should().Be(123);
        }

        [Fact]
        public async Task ShouldConsumeMultipleMessages()
        {
            // Arrange
            var sut = new ReadTunnelExample();
            var consumeRangeFunc = sut.ReadRangeExpr.Compile();

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            var data = consumeRangeFunc().ToList();

            // Assert
            data[0].Should().Be(3);  
            data[1].Should().Be(2);
            data[2].Should().Be(1);
        }
        
        [Fact]
        public async Task ShouldConsumeMultipleMessagesOnDemand()
        {
            // Arrange
            var sut = new ReadTunnelExample();
            var consumeRangeFunc = sut.ReadRangeExpr.Compile();

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            var data = consumeRangeFunc().Take(2).ToList();

            // Assert
            data.Count.Should().Be(2);
            data[0].Should().Be(3);  
            data[1].Should().Be(2);
        }
        
        [Fact]
        public async Task ShouldConsumeMessagesUsingCallback()
        {
            // Arrange
            var sut = new ReadTunnelExample();
            var consumeRangeFunc = sut.ConsumeWithCallbackExpr.Compile();
            var collectedData = new List<decimal>();

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            consumeRangeFunc((data) =>
            {
                collectedData.Add(data);
            });

            // Assert
            collectedData.Count.Should().Be(3);
            collectedData[0].Should().Be(3);  
            collectedData[1].Should().Be(2);
            collectedData[2].Should().Be(1);
        }  
        
        [Fact]
        public async Task ShouldConsumeMessagesUsingCallbackRange()
        {
            // Arrange
            var sut = new ReadTunnelExample();
            var consumeRangeFunc = sut.ConsumeRangeWithCallbackExpr.Compile();
            var collectedData = new List<decimal>();

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            consumeRangeFunc((data) =>
            {
                collectedData.AddRange(data);
            });

            // Assert
            collectedData.Count.Should().Be(3);
            collectedData[0].Should().Be(3);  
            collectedData[1].Should().Be(2);
            collectedData[2].Should().Be(1);
        }

        private class ReadTunnelExample : IReadDirectlyTunnel<decimal>, IReadViaCallbackTunnel<decimal>
        {
            public readonly ConcurrentStack<decimal> Stack = new();

            public Expression<Func<decimal>> ReadExpr => () => ConsumeLogic();

            private decimal ConsumeLogic()
            {
                decimal data;

                var success = Stack.TryPop(out data);

                if (!success) throw new NoDataException();

                return data;
            }

            public Expression<Func<IEnumerable<decimal>>> ReadRangeExpr => () => ConsumeRangeLogic();

            public Expression<Action<Action<decimal>>> ConsumeWithCallbackExpr =>
                (consumeCallback) => ConsumeWithCallbackLogic(consumeCallback);

            private void ConsumeWithCallbackLogic(Action<decimal> consumeCallback)
            {
                while (!Stack.IsEmpty)
                {
                    var success = Stack.TryPop(out var data);

                    if (success)
                    {
                        consumeCallback?.Invoke(data);
                    }
                }
            }

            public Expression<Action<Action<IEnumerable<decimal>>>> ConsumeRangeWithCallbackExpr => 
                (consumeCallback) =>
                    ConsumeRangeWithCallbackLogic(consumeCallback);

            private void ConsumeRangeWithCallbackLogic(Action<IEnumerable<decimal>> consumeCallback)
            {
                while (!Stack.IsEmpty)
                {
                    var success = Stack.TryPop(out var data);

                    if (success)
                    {
                        consumeCallback?.Invoke(new List<decimal>() { data });
                    }
                }
            }

            private IEnumerable<decimal> ConsumeRangeLogic()
            {
                while (!Stack.IsEmpty)
                {
                    var success = Stack.TryPop(out var data);

                    if (success)
                    {
                        yield return data;
                    }
                }
            }

            public bool CanExecute { get; set; } = true;
        }
    }
}