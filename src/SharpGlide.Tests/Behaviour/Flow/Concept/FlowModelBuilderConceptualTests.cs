using System;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Tests.Behaviour.Flow.Concept.Examples;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Flow.Concept
{
    public class FlowModelBuilderConceptualTests
    {
        [Fact]
        public async Task ShouldBuildModelBasedOnConfiguration()
        {
            // Arrange
            var sut = new ModelBuilder();

            // Act
            var model = sut.Configure<SampleConfigurationProvider>(
                new[]
                {
                    nameof(IntToStringPart)
                },
                new[]
                {
                    nameof(IntReader),
                    nameof(StringWriter)
                },
                new[] { string.Empty });

            // Assert
            model.Parts.Count.Should().Be(1);
        }

        public class SampleConfigurationProvider : IConfigurationEntryProvider
        {
            public ConfigurationEntry Parse(string contents)
            {
                switch (contents)
                {
                    case nameof(IntToStringPart):
                        return new ConfigurationEntry()
                        {
                            FullName = typeof(IntToStringPart).FullName,
                            AssemblyLocation = typeof(IntToStringPart).Assembly.Location
                        };
                    case nameof(IntReader):
                        return new ConfigurationEntry()
                        {
                            FullName = typeof(IntReader).FullName,
                            AssemblyLocation = typeof(IntReader).Assembly.Location
                        };
                    case nameof(StringWriter):
                        return new ConfigurationEntry()
                        {
                            FullName = typeof(StringWriter).FullName,
                            AssemblyLocation = typeof(StringWriter).Assembly.Location
                        };
                    default:
                        throw new ArgumentOutOfRangeException(nameof(contents));
                }
            }

            public string Read(string pointer)
            {
                return pointer;
            }
        }
    }
}