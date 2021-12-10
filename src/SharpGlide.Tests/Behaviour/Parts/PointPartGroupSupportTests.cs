using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Behaviours;
using SharpGlide.Tests.Model.PointPart;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
{
    public class PointPartGroupSupportTests
    {
        [Fact]
        public async Task PointPartStartsChildrenInParallelWay()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport { Name = "Root"};
            var childrenAWithChild = new TestPointPartWith1SecondExecution() { Name = "Child 1"};
            var childrenB = new TestPointPartWith1SecondExecution { Name = "Child 2"};
            var childrenC = new TestPointPartWith1SecondExecution { Name = "Child 3"};
            var childrenD = new TestPointPartWith1SecondExecution { Name = "Child 4"};
            
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
            var sw = new Stopwatch();
            sw.Start();
            await rootPart.StartAsync();
            sw.Stop();

            // Assert
            (sw.Elapsed.TotalSeconds < 3).Should().BeTrue(); // we expect that execution time will be lower than total count of children
            rootPart.TestProperty.Should().Be("Started");
            childrenAWithChild.TestProperty.Should().Be("Completed");
            childrenB.TestProperty.Should().Be("Completed");
            childrenC.TestProperty.Should().Be("Completed");
            childrenD.TestProperty.Should().Be("Completed");
        }
        
        [Fact]
        public async Task PointPartChildrenAddsWithDefaultName()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport();
            var childrenB = new TestPointPartWithGroupSupport ();
            var childrenC = new TestPointPartWithGroupSupport();
            var childrenD = new TestPointPartWithGroupSupport();
            
            rootPart.ConfigureStartAs<StartInBackground>();
            childrenAWithChild.ConfigureStartAs<StartInBackground>();
            childrenB.ConfigureStartAs<StartInBackground>();
            childrenC.ConfigureStartAs<StartInBackground>();
            childrenD.ConfigureStartAs<StartInBackground>();
            
            // Act
            rootPart.AddChild(childrenAWithChild);
            childrenAWithChild.AddChild(childrenB);
            childrenAWithChild.AddChild(childrenC);
            rootPart.AddChild(childrenD);

            // Assert
            rootPart.Name.Should().Be("Root");
            childrenAWithChild.Name.Should().Be("TestPointPartWithGroupSupport");
            childrenB.Name.Should().Be("TestPointPartWithGroupSupport");
            childrenC.Name.Should().Be("Copy of TestPointPartWithGroupSupport");
            childrenD.Name.Should().Be("Copy of TestPointPartWithGroupSupport");
        }
        
        [Fact]
        public async Task PointPartStartsChildrenOnceStarted()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport { Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport { Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport { Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport { Name = "Child 4"};
            
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

            // Assert
            rootPart.TestProperty.Should().Be("Started");
            childrenAWithChild.TestProperty.Should().Be("Started");
            childrenB.TestProperty.Should().Be("Started");
            childrenC.TestProperty.Should().Be("Started");
            childrenD.TestProperty.Should().Be("Started");
        }
        
        [Fact]
        public async Task PointPartCanReturnDirectChildren()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport { Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport { Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport { Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport { Name = "Child 4"};
            
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
        public async Task PointPartShouldBuildPartTree()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport { Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport { Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport { Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport { Name = "Child 4"};
            
            rootPart.AddChild(childrenAWithChild);
            childrenAWithChild.AddChild(childrenB);
            childrenAWithChild.AddChild(childrenC);
            rootPart.AddChild(childrenD);

            // Act & Assert
            var partTree = rootPart.Context.GroupContext.GetPartTree(rootPart);

            partTree.ElementAt(0).Item2.Should().Be(rootPart);
            partTree.ElementAt(1).Item2.Should().Be(childrenAWithChild);
            partTree.ElementAt(2).Item2.Should().Be(childrenB);
            partTree.ElementAt(3).Item2.Should().Be(childrenC);
            partTree.ElementAt(4).Item2.Should().Be(childrenD);
            
            partTree.ElementAt(0).Item1.Should().Be(0);
            partTree.ElementAt(1).Item1.Should().Be(1);
            partTree.ElementAt(2).Item1.Should().Be(2);
            partTree.ElementAt(3).Item1.Should().Be(2);
            partTree.ElementAt(4).Item1.Should().Be(1);
        }
        
        [Fact]
        public async Task PointPartCanReturnDeepChildrenUsingRecursive()
        {
            // Arrange
            var rootPart = new TestPointPartWithGroupSupport { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport { Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport { Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport { Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport { Name = "Child 4"};
            
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
            var rootPart = new TestPointPartWithGroupSupport { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport { Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport { Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport { Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport { Name = "Child 4"};
            
            rootPart.AddChild(childrenAWithChild);
            rootPart.AddChild(childrenB);
            rootPart.AddChild(childrenC);
            rootPart.AddChild(childrenD);

            // Act
            var childrenNames = new List<string>();
            rootPart.EnumerateChildren(part =>
            {
                childrenNames.Add(part.Name);
            });
            
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
            var rootPart = new TestPointPartWithGroupSupport { Name = "Root"};
            var childrenAWithChild = new TestPointPartWithGroupSupport { Name = "Child 1"};
            var childrenB = new TestPointPartWithGroupSupport { Name = "Child 2"};
            var childrenC = new TestPointPartWithGroupSupport { Name = "Child 3"};
            var childrenD = new TestPointPartWithGroupSupport { Name = "Child 4"};
            
            rootPart.AddChild(childrenAWithChild);
            childrenAWithChild.AddChild(childrenB);
            childrenAWithChild.AddChild(childrenC);
            childrenAWithChild.AddChild(childrenD);

            // Act
            var childrenNames = new List<string>();
            rootPart.EnumerateChildren(part =>
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