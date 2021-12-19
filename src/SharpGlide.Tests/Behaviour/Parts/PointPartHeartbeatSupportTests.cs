using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Behaviours;
using SharpGlide.Extensions;
using SharpGlide.Tests.Model.PointPart;
using SharpGlide.Tests.Model.Tunnel;
using SharpGlide.Tunnels.Routes;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
{
    public class PointPartHeartbeatSupportTests
    {
        [Fact]
        public async Task PointPartShouldReportHeartBeatOnceRequested()
        {
            // Arrange
            var sut = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport { Name = "Root" };
            var testWorkingContext = new TestWorkingContext();
            sut.ConfigureHeartBeatTunnel<StringListPublishTunnel, TestWorkingContext>(
                (context, tunnel, part) =>
                {
                    context.TestEvent += (sender, s) =>
                    {
                       var heartBeat = part.GetHeartBeat();
                       tunnel.Publish(heartBeat, PublishRoute.Default);
                    };
                },
                testWorkingContext);
            
            // Act
            testWorkingContext.OnTestEvent();

            // Assert
            var tunnel = sut.Context.HeartBeatContext.HeartBeatTunnel as StringListPublishTunnel;
            tunnel?.PublishedData.Should().NotBeEmpty();
            var reportAsXml = tunnel?.PublishedData.First();
            reportAsXml.Should().Contain("Root");
        }

        [Fact]
        public async Task PointPartShouldGetStatusAsXml()
        {
            // Arrange 
            var rootPart = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport { Name = "Root" };
            var childrenAWithChild = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport { Name = "Child 1" };
            var childrenB = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport { Name = "Child 2" };
            var childrenC = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport { Name = "Child 3" };
            var childrenD = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport { Name = "Child 4" };

            rootPart.ConfigureStartAs<StartInBackground>();
            childrenAWithChild.ConfigureStartAs<StartInBackground>();
            childrenB.ConfigureStartAs<StartInBackground>();
            childrenC.ConfigureStartAs<StartInBackground>();
            childrenD.ConfigureStartAs<StartInBackground>();

            rootPart.AddChild(childrenAWithChild);
            childrenAWithChild.AddChild(childrenB);
            childrenAWithChild.AddChild(childrenC);
            rootPart.AddChild(childrenD);

            // Act
            await rootPart.StartAsync();
            var heartBeat = rootPart.GetHeartBeat().AsXml();

            // Assert
            heartBeat.Should().Contain("Root");
            heartBeat.Should().Contain("Child 1");
            heartBeat.Should().Contain("Child 2");
            heartBeat.Should().Contain("Child 3");
            heartBeat.Should().Contain("Child 4");
        }
        
        [Fact]
        public async Task PointPartShouldPrintExceptionList()
        {
            // Arrange 
            var rootPart = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport { Name = "Root" };
            var childrenAWithChild = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport { Name = "Child 1" };
            var childrenB = new TestPointPartWithFailure() { Name = "Child 2" };
            var childrenC = new TestPointPartWithFailure { Name = "Child 3" };
            var childrenD = new TestPointPartWithFailure { Name = "Child 4" };

            rootPart.AddChild(childrenAWithChild);
            childrenAWithChild.AddChild(childrenB);
            childrenAWithChild.AddChild(childrenC);
            rootPart.AddChild(childrenD);

            // Act
            await rootPart.StartAsync();
            var reportAsXml = rootPart.GetHeartBeat().AsXml();

            // Assert
            reportAsXml.Should().Contain("Failure of Child 2: System.Exception: Some Exception");
            reportAsXml.Should().Contain("Failure of Child 3: System.Exception: Some Exception");
            reportAsXml.Should().Contain("Failure of Child 4: System.Exception: Some Exception");

            reportAsXml.Should().NotContain("Failure of Child 1: System.Exception: Some Exception");
            reportAsXml.Should().NotContain("Failure of Root: System.Exception: Some Exception");
        }


        private class TestWorkingContext
        {
            public event EventHandler TestEvent;

            public void OnTestEvent()
            {
                TestEvent?.Invoke(this, null);
            }
        }
    }
}