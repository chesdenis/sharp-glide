using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Extensions;
using Xunit;

namespace SharpGlide.Tests.Unit.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public async Task ShouldTrimLargeText()
        {
            // Arrange
            var input = "Some large text";

            // Act & Assert
            input.CutIfMoreCharacters(-1).Should().Be(input);
            input.CutIfMoreCharacters(1000).Should().Be(input);
            input.CutIfMoreCharacters(5).Should().Be("So...xt");
            input.CutIfMoreCharacters(12).Should().Be("Some l...e text");
        }
    }
}