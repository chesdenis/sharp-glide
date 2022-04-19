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
        public async Task ShouldReadSingleMessage()
        {
            // Arrange
            var sut = new DirectReaderExample();
            var func = sut.ReadExpr.Compile();

            // Act
            sut.Stack.Push(123);
            var data = func();

            // Assert
            data.Should().Be(123);
        }

        [Fact]
        public async Task ShouldReadMultipleData()
        {
            // Arrange
            var sut = new DirectReaderExample();
            var func = sut.ReadRangeExpr.Compile();

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            var data = func().ToList();

            // Assert
            data[0].Should().Be(3);
            data[1].Should().Be(2);
            data[2].Should().Be(1);
        }

        [Fact]
        public async Task ShouldReadMultipleDataOnDemand()
        {
            // Arrange
            var sut = new DirectReaderExample();
            var func = sut.ReadRangeExpr.Compile();

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            var data = func().Take(2).ToList();

            // Assert
            data.Count.Should().Be(2);
            data[0].Should().Be(3);
            data[1].Should().Be(2);
        }

        [Fact]
        public async Task ShouldReadDataUsingCallback()
        {
            // Arrange
            var sut = new DirectReaderExample();
            var func = sut.ReadViaCallbackExpr.Compile();
            var collectedData = new List<decimal>();

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            func(data => collectedData.Add(data));

            // Assert
            collectedData.Count.Should().Be(3);
            collectedData[0].Should().Be(3);
            collectedData[1].Should().Be(2);
            collectedData[2].Should().Be(1);
        }

        [Fact]
        public async Task ShouldReadDataUsingCallbackRange()
        {
            // Arrange
            var sut = new DirectReaderExample();
            var func = sut.ReadRangeViaCallbackExpr.Compile();
            var collectedData = new List<decimal>();

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            func((data) => { collectedData.AddRange(data); });

            // Assert
            collectedData.Count.Should().Be(3);
            collectedData[0].Should().Be(3);
            collectedData[1].Should().Be(2);
            collectedData[2].Should().Be(1);
        }

        [Fact]
        public async Task ShouldReadDataUsingEvent()
        {
            // Arrange
            var sut = new DirectReaderExample();
            var func = sut.ReadViaEventExpr.Compile();
            var collectedData = new List<decimal>();

            sut.OnRead += (sender, e) => collectedData.Add(e);

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            func();

            // Assert
            collectedData.Count.Should().Be(3);
            collectedData[0].Should().Be(3);
            collectedData[1].Should().Be(2);
            collectedData[2].Should().Be(1);
        }
        
        
        [Fact]
        public async Task ShouldReadDataRangeUsingEvent()
        {
            // Arrange
            var sut = new DirectReaderExample();
            var func = sut.ReadRangeViaEventCallbackExpr.Compile();
            var collectedData = new List<decimal>();

            sut.OnReadRange += (sender, e) => collectedData.AddRange(e);

            // Act
            sut.Stack.Push(1);
            sut.Stack.Push(2);
            sut.Stack.Push(3);
            func();

            // Assert
            collectedData.Count.Should().Be(3);
            collectedData[0].Should().Be(3);
            collectedData[1].Should().Be(2);
            collectedData[2].Should().Be(1);
        }

        private class DirectReaderExample : IDirectReader<decimal>, IReaderViaCallback<decimal>, IReaderViaEvent<decimal>
        {
            public readonly ConcurrentStack<decimal> Stack = new();

            public event EventHandler<decimal> OnRead;
            public event EventHandler<IEnumerable<decimal>> OnReadRange;

            public Expression<Func<decimal>> ReadExpr => () => ConsumeLogic();

            private decimal ConsumeLogic()
            {
                decimal data;

                var success = Stack.TryPop(out data);

                if (!success) throw new NoDataException();

                return data;
            }

            public Expression<Func<IEnumerable<decimal>>> ReadRangeExpr => () => ConsumeRangeLogic();

            public Expression<Action<Action<decimal>>> ReadViaCallbackExpr =>
                (consumeCallback) => ConsumeWithCallbackLogic(consumeCallback);

            public Expression<Action> ReadViaEventExpr => () => ConsumeViaEventLogic();

            private void ConsumeViaEventLogic()
            {
                while (!Stack.IsEmpty)
                {
                    var success = Stack.TryPop(out var data);

                    if (success)
                    {
                        OnRead?.Invoke(this, data);
                    }
                }
            }


            public Expression<Action> ReadRangeViaEventCallbackExpr => () => ConsumeRangeViaEventLogic();

            private void ConsumeRangeViaEventLogic()
            {
                var range = new List<decimal>();
                while (!Stack.IsEmpty)
                {
                    var success = Stack.TryPop(out var data);

                    if (success)
                    {
                        range.Add(data);
                    }
                }

                OnReadRange?.Invoke(this, range);
            }

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

            public Expression<Action<Action<IEnumerable<decimal>>>> ReadRangeViaCallbackExpr => 
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