using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            var testQueue = new Queue<byte[]>();
            
            var partRegistry = new PartsRegistry();
            var sut = new SampleAppA();
            partRegistry.RegisterPart("app1", sut);
            sut.RegisterRaiseUpBehaviour<SampleExecuteSequentiallyRaiseUpBehavior>();
            sut.RegisterPublishTunnel(() => new SampleInMemoryPublishTunnel(testQueue));
 
            // Act
            sut.Start();

            // Assert
            sut.PropertyA.Should().Be("Started");
            testQueue.Dequeue().FromBytes<string>().Should().Be("Started");
        }

        [Fact]
        public void ShouldPartConsumeDataFromPublisher()
        {
            // Arrange
            var testQueue = new Queue<byte[]>();
            
            var partRegistry = new PartsRegistry();
            var sampleAppA = new SampleAppA();
            var sampleAppB = new SampleAppB();
            partRegistry.RegisterPart("app1", sampleAppA);
            partRegistry.RegisterPart("app2", sampleAppB);
            
            sampleAppA.RegisterRaiseUpBehaviour<SampleExecuteSequentiallyRaiseUpBehavior>();
            sampleAppB.RegisterRaiseUpBehaviour<SampleExecuteSequentiallyRaiseUpBehavior>();

            sampleAppA.RegisterPublishTunnel(() => new SampleInMemoryPublishTunnel(testQueue));
            sampleAppB.RegisterConsumeTunnel(() => new SampleInMemoryConsumeTunnel(testQueue));
            
            // Act
            sampleAppA.Start();
            sampleAppB.Start();

            // Assert
            sampleAppA.PropertyA.Should().Be("Started");
            sampleAppB.PropertyB.Should().Be("Started");
        }
        
        private class SampleInMemoryPublishTunnel : IPublishTunnel
        {
            private readonly Queue<byte[]> _queue;

            public SampleInMemoryPublishTunnel(Queue<byte[]> queue)
            {
                _queue = queue;
            }
            public bool CanPublishThis(byte[] data) => true;

            public void Publish(byte[] data)
            {
                _queue.Enqueue(data);
            }
        }
        
        private class SampleInMemoryConsumeTunnel : IConsumeTunnel
        {
            private readonly Queue<byte[]> _queue;

            public SampleInMemoryConsumeTunnel(Queue<byte[]> queue)
            {
                _queue = queue;
            }
             
            public byte[] Receive() => _queue.Dequeue();
        }
        
        private class SampleExecuteSequentiallyRaiseUpBehavior : IRaiseUpBehaviour
        {
            public void Execute(IDataFlowPart part)
            {
                part.EntryPointer()();
            }
        }
        
        private class SampleAppA : DataFlowPart
        {
            public string PropertyA { get; set; }

            protected override void OnEntry()
            {
                PropertyA = "Started";
                
                this.Publish(PropertyA.AsBytes());
            }
        }
        
        private class SampleAppB : DataFlowPart
        {
            public string PropertyB { get; set; }

            protected override void OnEntry()
            {
                this.Receive(data =>
                {
                    PropertyB = data.FromBytes<string>();
                });
            }
        }

    }
}