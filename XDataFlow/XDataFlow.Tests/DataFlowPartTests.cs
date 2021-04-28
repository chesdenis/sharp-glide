using System;
using FluentAssertions;
using XDataFlow.Behaviours;
using XDataFlow.Extensions;
using XDataFlow.Parts;
using Xunit;

namespace XDataFlow.Tests
{
    public class DataFlowPartTests
    {
        [Fact]
        public void ShouldPartNotToBeRegisteredOnceWeHaveItAlready()
        {
            // Arrange
            var partRegistry = new PartsRegistry();
            var sut = new SampleAppA();
            
            // Act
            partRegistry.RegisterPart("app1", sut);
            partRegistry.RegisterPart("app2", sut);

            // Assert
            new Action(() =>
            {
                partRegistry.RegisterPart("app1", sut);
            }).Should().Throw<ArgumentException>();
        }
        
        
        [Fact]
        public void ShouldPartSupportRaiseUpBehaviour()
        {
            // Arrange
            var partRegistry = new PartsRegistry();
            var sut = new SampleAppA();
            partRegistry.RegisterPart("app1", sut);
            sut.RegisterRaiseUpBehaviour<SampleExecuteSequentiallyRaiseUpBehavior>();

            // Act
            sut.Start();

            // Assert
            sut.PropertyA.Should().Be("Started");
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
            }
        }
    }
}