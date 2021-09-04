using System.Threading.Tasks;
using FluentAssertions;
using XDataFlow.Behaviours;
using XDataFlow.Tests.Model;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class PointPartHeartbeatSupportTests
    {
        [Fact]
        public async Task PointPartShouldPrintStatusTree()
        {
            // Arrange 
            var rootPart = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport() { Name = "Root" };
            var childrenAWithChild = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport() { Name = "Child 1" };
            var childrenB = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport() { Name = "Child 2" };
            var childrenC = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport() { Name = "Child 3" };
            var childrenD = new TestPointPartWithGroupAndMetadataAndHeartbeatSupport() { Name = "Child 4" };

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
            statusTable.Should().Contain("----------------------------------------------------");
            statusTable.Should().Contain("| Name      | Available | _ETA     | _Speed, n/sec |");
            statusTable.Should().Contain("----------------------------------------------------");
            statusTable.Should().Contain("| Root      | 0         | 00:00:00 | 0             |");
            statusTable.Should().Contain("----------------------------------------------------");
            statusTable.Should().Contain("| -Child 1  | 0         | 00:00:00 | 0             |");
            statusTable.Should().Contain("----------------------------------------------------");
            statusTable.Should().Contain("| --Child 2 | 0         | 00:00:00 | 0             |");
            statusTable.Should().Contain("----------------------------------------------------");
            statusTable.Should().Contain("| --Child 3 | 0         | 00:00:00 | 0             |");
            statusTable.Should().Contain("----------------------------------------------------");
            statusTable.Should().Contain("| -Child 4  | 0         | 00:00:00 | 0             |");
            statusTable.Should().Contain("----------------------------------------------------");
        }
    }
}