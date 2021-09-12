using System.Threading.Tasks;
using SharpGlide.Behaviours;
using SharpGlide.Tests.Model;
using FluentAssertions;
using Xunit;

namespace SharpGlide.Tests.Parts
{
    public class PointPartHeartbeatSupportTests
    {
        [Fact]
        public async Task PointPartShouldPrintStatusTree()
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
            var statusTable = rootPart.GetStatusTable();

            // Assert
            statusTable.Should().Contain("Root");
            statusTable.Should().Contain("-------------");
            statusTable.Should().Contain("| Name      |");
            statusTable.Should().Contain("-------------");
            statusTable.Should().Contain("| Root      |");
            statusTable.Should().Contain("-------------");
            statusTable.Should().Contain("| -Child 1  |");
            statusTable.Should().Contain("-------------");
            statusTable.Should().Contain("| --Child 2 |");
            statusTable.Should().Contain("-------------");
            statusTable.Should().Contain("| --Child 3 |");
            statusTable.Should().Contain("-------------");
            statusTable.Should().Contain("| -Child 4  |");
            statusTable.Should().Contain("-------------");
        }
    }
}