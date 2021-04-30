using System;
using System.Collections.Generic;
using FluentAssertions;
using XDataFlow.Behaviours;
using XDataFlow.Extensions;
using XDataFlow.Parts;
using XDataFlow.Tunnels;
using Xunit;

namespace XDataFlow.Tests
{
    public class DataFlowTunnelsTests
    {
        [Fact]
        public void ShouldPartPublishDataOncePublisherHasBeenRegistered()
        {
            // Arrange
            var testQueue = new Queue<SampleData>();
            
            var partRegistry = new PartsRegistry();
            var sut = new SampleAppA();
            partRegistry.RegisterPart("app1", sut);
            sut.RegisterRaiseUpBehaviour<SampleExecuteSequentiallyRaiseUpBehavior>();
            sut.RegisterPublishTunnel(() => new SampleInMemoryPublishTunnel<SampleData>(testQueue));
 
            // Act
            sut.Start();

            // Assert
            sut.Data.PropertyX.Should().Be("1");
            sut.Data.PropertyY.Should().Be("2");
            sut.Data.PropertyZ.Should().Be(3);
            
            testQueue.Dequeue().Should().Be(sut.Data);
        }
        
        [Fact]
        public void ShouldPartConsumeDataFromPublisher()
        {
            // Arrange
            var testQueue = new Queue<SampleData>();
            
            var partRegistry = new PartsRegistry();
            var sampleAppA = new SampleAppA();
            var sampleAppB = new SampleAppB();
            partRegistry.RegisterPart("app1", sampleAppA);
            partRegistry.RegisterPart("app2", sampleAppB);
            
            sampleAppA.RegisterRaiseUpBehaviour<SampleExecuteSequentiallyRaiseUpBehavior>();
            sampleAppB.RegisterRaiseUpBehaviour<SampleExecuteSequentiallyRaiseUpBehavior>();

            sampleAppA.RegisterPublishTunnel(() => new SampleInMemoryPublishTunnel<SampleData>(testQueue));
            sampleAppB.RegisterConsumeTunnel(() => new SampleInMemoryConsumeTunnel<SampleData>(testQueue));
            
            // Act
            sampleAppA.Start();
            sampleAppB.Start();

            // Assert
            sampleAppA.Data.PropertyX.Should().Be("1");
            sampleAppA.Data.PropertyY.Should().Be("2");
            sampleAppA.Data.PropertyZ.Should().Be(3);
            
            sampleAppB.Data.PropertyX.Should().Be("1");
            sampleAppB.Data.PropertyY.Should().Be("2");
            sampleAppB.Data.PropertyZ.Should().Be(3);
        }

        [Fact]
        public void ShouldPartConsumeAndPublishData()
        {
            // Arrange
            var testQueueIn = new Queue<int>();
            var testQueueOut = new Queue<SampleData>();
            var partRegistry = new PartsRegistry();
            
            var sampleAppC = new SampleAppC();
            partRegistry.RegisterPart("app1", sampleAppC);
            sampleAppC.RegisterRaiseUpBehaviour<SampleExecuteSequentiallyRaiseUpBehavior>();
            
            sampleAppC.RegisterPublishTunnel(() => new SampleInMemoryPublishTunnel<SampleData>(testQueueOut));
            sampleAppC.RegisterConsumeTunnel(() => new SampleInMemoryConsumeTunnel<int>(testQueueIn));
            
            testQueueIn.Enqueue(1);
            
            // Act
            sampleAppC.Start();
            
            // Assert
            var outData = testQueueOut.Dequeue();
            outData.PropertyZ.Should().Be(1);
        }

        private class SampleData
        {
            public string PropertyX { get; set; }
            public string PropertyY { get; set; }
            public int PropertyZ { get; set; }
        }
        
        private class SampleInMemoryPublishTunnel<T> : PublishTunnel<T>
        {
            private readonly Queue<T> _queue;

            public SampleInMemoryPublishTunnel(Queue<T> queue)
            {
                _queue = queue;
            }

            public override Action<T> PublishPointer() => (data) =>
            {
                _queue.Enqueue(data);
            };
        }
        
        private class SampleInMemoryConsumeTunnel<T> : ConsumeTunnel<T>
        {
            private readonly Queue<T> _queue;

            public SampleInMemoryConsumeTunnel(Queue<T> queue)
            {
                _queue = queue;
            }

            public override Func<T> ConsumePointer()
            {
                return () => _queue.Dequeue();
            }
        }
         
        private class SampleExecuteSequentiallyRaiseUpBehavior : IRaiseUpBehaviour
        {
            public void Execute(IDataFlowPart part)
            {
                part.EntryPointer()();
            }
        }
        
        private class SampleAppA : PublisherOnlyFlowPart<SampleData>
        {
            public SampleData Data { get; set; }

            protected override void OnEntry()
            {
                Data = new SampleData()
                {
                    PropertyX = "1",
                    PropertyY =  "2",
                    PropertyZ = 3
                };
                
                this.Publish(Data);
            }
        }
        
        private class SampleAppB : ConsumerOnlyFlowPart<SampleData>
        {
            public SampleData Data { get; set; }

            protected override void OnEntry()
            {
                this.Consume<SampleAppB, SampleData>((data) =>
                {
                    Data = data;
                });
            }
        }
        
        private class SampleAppC : PublisherConsumerFlowPart<SampleData, int>
        {
            protected override void OnEntry()
            {
                 this.Consume<SampleAppC, SampleData, int>((data) =>
                 {
                     this.Publish<SampleAppC, SampleData, int>(new SampleData() {PropertyZ = data});
                 });
            }
        }
    }
}