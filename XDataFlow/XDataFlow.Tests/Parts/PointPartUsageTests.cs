using System;
using System.Threading;
using Moq;
using XDataFlow.Parts.Interfaces;
using XDataFlow.Refactored;
using XDataFlow.Refactored.Builders;
using Xunit;

namespace XDataFlow.Tests.Parts
{
    public class PointPartUsageTests
    {
        [Fact]
        void NameMeSomeHow()
        {
            // Arrange
           

            // Act
            var partTemplate = PartDefaultBuilder.GetTemplate<TestPointPart>();
            var part = partTemplate();
            part.SwitchController.OnStart();

            // Assert
        }
        
        public static PointPartBuilder PartDefaultBuilder = new PointPartBuilder(
            (() => new Mock<IMetaDataController>().Object),
            () => new Mock<IGroupController>().Object);

        public class TestPointPart : PointPart
        {
            public string TestProperty { get; set; }

            public TestPointPart(
                IMetaDataController metaDataController,
                IGroupController groupController) : base(metaDataController, groupController)
            {
            }
            
            public override void Process(CancellationToken cancellationToken)
            {
                this.TestProperty = "ABCDE";
            }
        }
    }
}