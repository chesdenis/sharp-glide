using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using XDataFlow.Tests.Model;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class PointPartGroupSupportTests
    {
        [Fact]
        public async Task PointPartCanReturnDirectChildren()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport() { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport(){ Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport(){ Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport(){ Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport(){ Name = "Child 4"};
            
            rootPart.AddChild(childrenAWithChild);
            rootPart.AddChild(childrenB);
            rootPart.AddChild(childrenC);
            rootPart.AddChild(childrenD);

            // Act & Assert
            rootPart.GetChild("Child 1").Should().Be(childrenAWithChild);
            rootPart.GetChild("Child 2").Should().Be(childrenB);
            rootPart.GetChild("Child 3").Should().Be(childrenC);
            rootPart.GetChild("Child 4").Should().Be(childrenD);
        }
        
        [Fact]
        public async Task PointPartCanReturnDeepChildrenUsingRecursive()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport() { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport(){ Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport(){ Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport(){ Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport(){ Name = "Child 4"};
            
            rootPart.AddChild(childrenAWithChild);
            childrenAWithChild.AddChild(childrenB);
            childrenAWithChild.AddChild(childrenC);
            childrenAWithChild.AddChild(childrenD);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => rootPart.GetChild("Child 2"));
            Assert.Throws<KeyNotFoundException>(() => rootPart.GetChild("Child 3"));
            Assert.Throws<KeyNotFoundException>(() => rootPart.GetChild("Child 4"));
            Assert.Throws<KeyNotFoundException>(() => rootPart.GetChild("Not Exist Child", true));

            rootPart.GetChild("Child 2", true).Should().Be(childrenB);
            rootPart.GetChild("Child 3", true).Should().Be(childrenC);
            rootPart.GetChild("Child 4", true).Should().Be(childrenD);
        }

        [Fact]
        public async Task PointPartCanIterateThroughDirectChildren()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport() { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport(){ Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport(){ Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport(){ Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport(){ Name = "Child 4"};
            
            rootPart.AddChild(childrenAWithChild);
            rootPart.AddChild(childrenB);
            rootPart.AddChild(childrenC);
            rootPart.AddChild(childrenD);

            // Act
            var childrenNames = new List<string>();
            rootPart.EnumerateParts((part) =>
            {
                childrenNames.Add(part.Name);
            }, false);
            
            // Assert
            childrenNames.Count.Should().Be(4);
            childrenNames.Should().Contain("Child 1");
            childrenNames.Should().Contain("Child 2");
            childrenNames.Should().Contain("Child 3");
            childrenNames.Should().Contain("Child 4");
        }
        
        [Fact]
        public async Task PointPartCanIterateThroughAllChildren()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport() { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport(){ Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport(){ Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport(){ Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport(){ Name = "Child 4"};
            
            rootPart.AddChild(childrenAWithChild);
            childrenAWithChild.AddChild(childrenB);
            childrenAWithChild.AddChild(childrenC);
            childrenAWithChild.AddChild(childrenD);

            // Act
            var childrenNames = new List<string>();
            rootPart.EnumerateParts((part) =>
            {
                childrenNames.Add(part.Name);
            }, true);
            
            // Assert
            childrenNames.Count.Should().Be(4);
            childrenNames.Should().Contain("Child 1");
            childrenNames.Should().Contain("Child 2");
            childrenNames.Should().Contain("Child 3");
            childrenNames.Should().Contain("Child 4");
        }
    }
}