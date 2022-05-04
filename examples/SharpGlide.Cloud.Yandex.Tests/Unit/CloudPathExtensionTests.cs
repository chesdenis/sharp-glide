using FluentAssertions;
using SharpGlide.Cloud.Yandex.Extensions;
using Xunit;

namespace SharpGlide.Cloud.Yandex.Tests.Unit
{
    public class CloudPathExtensionTests
    {
        [Theory]
        [InlineData("C:\\test", "C:\\test\\subfolder2")]
        [InlineData("C:\\test\\", "C:\\test\\subfolder2\\")]
        [InlineData("C:\\test\\\\", "C:\\test\\subfolder2\\")]
        public void ShouldCalculateRelativePathForWindows(string rootPath, string filePath)
        {
            // Arrange
            // Act
            var result = filePath.CalculateRelativePath(rootPath);

            // Assert
            result.Should().Be("/subfolder2");
        }  
        
        [Theory]
        [InlineData("/test/", "/test/subfolder2")]
        [InlineData("/test/", "//test/subfolder2/")]
        [InlineData("/test//", "test/subfolder2")]
        [InlineData("//test/", "//test/subfolder2")]
        public void ShouldCalculateRelativePathForUnix(string rootPath, string filePath)
        {
            // Arrange
            // Act
            var result = filePath.CalculateRelativePath(rootPath);

            // Assert
            result.Should().Be("/subfolder2");
        }
    }
}