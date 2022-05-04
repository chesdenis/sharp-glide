using FluentAssertions;
using SharpGlide.Cloud.Yandex.Extensions;
using Xunit;

namespace SharpGlide.Cloud.Yandex.Tests.Unit
{
    public class CloudPathExtensionTests
    {
        [Fact]
        public void ShouldEncodeUnixPath()
        {
            // Arrange
            // Act
            var result = "/testFolder1/subfolder2".ToCloudPath();

            // Assert
            result.Should().Be("%2ftestFolder1%2fsubfolder2");
        } 
        
        [Fact]
        public void ShouldEncodeWindowsPath()
        {
            // Arrange
            // Act
            var result = "C:\\testfodler\\subfolder2".ToCloudPath();

            // Assert
            result.Should().Be("C%3a%2ftestfodler%2fsubfolder2");
        }
        
        [Fact]
        public void ShouldCalculateRelativePathForWindows()
        {
            // Arrange
            var rootPath = "C:\\testfodler";
            // Act
            var result = "C:\\testfodler\\subfolder2".CalculateRelativePath(rootPath).ToCloudPath();

            // Assert
            result.Should().Be("%2fsubfolder2");
        }  
        
        [Fact]
        public void ShouldCalculateRelativePathForUnix()
        {
            // Arrange
            var rootPath = "/testFolder/";
            // Act
            var result = "/testFolder/subfolder2".CalculateRelativePath(rootPath).ToCloudPath();

            // Assert
            result.Should().Be("%2fsubfolder2");
        }
    }
}