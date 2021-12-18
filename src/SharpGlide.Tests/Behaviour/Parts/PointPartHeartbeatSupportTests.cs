using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Behaviours;
using SharpGlide.Tests.Model.PointPart;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
{
    public class PointPartHeartbeatSupportTests
    {
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
            var reportAsXml = rootPart.ReportAsXml();

            // Assert
            reportAsXml.Should().Contain("Root");
            reportAsXml.Should().Contain("Child 1");
            reportAsXml.Should().Contain("Child 2");
            reportAsXml.Should().Contain("Child 3");
            reportAsXml.Should().Contain("Child 4");
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
            var reportAsXml = rootPart.ReportAsXml();

            // Assert
            reportAsXml.Should().Contain("Failure of Child 2: System.Exception: Some Exception");
            reportAsXml.Should().Contain("Failure of Child 3: System.Exception: Some Exception");
            reportAsXml.Should().Contain("Failure of Child 4: System.Exception: Some Exception");

            reportAsXml.Should().NotContain("Failure of Child 1: System.Exception: Some Exception");
            reportAsXml.Should().NotContain("Failure of Root: System.Exception: Some Exception");
        }
    }
}