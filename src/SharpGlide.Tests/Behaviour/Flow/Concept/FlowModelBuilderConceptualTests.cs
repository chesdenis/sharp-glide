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
        // [Fact]
        // public async Task ShouldBuildModelBasedOnConfiguration()
        // {
        //     // Arrange
        //     var sut = new FlowModelBuilder();
        //
        //     // Act
        //     var model = sut.Configure<SampleFlowModelProvider>(
        //         new[]
        //         {
        //             nameof(IntToStringPart)
        //         },
        //         new[]
        //         {
        //             nameof(IntReadTunnel),
        //             nameof(WriteStringTunnel)
        //         },
        //         new[] { string.Empty });
        //
        //     // Assert
        //     model.Parts.Count.Should().Be(1);
        // }
        //
        // public class SampleFlowModelProvider : IFlowModelEntryProvider
        // {
        //     public ConfigurationEntry Parse(string contents)
        //     {
        //         switch (contents)
        //         {
        //             case nameof(IntToStringPart):
        //                 return new ConfigurationEntry()
        //                 {
        //                     FullName = typeof(IntToStringPart).FullName,
        //                     AssemblyLocation = typeof(IntToStringPart).Assembly.Location
        //                 };
        //             case nameof(IntReadTunnel):
        //                 return new ConfigurationEntry()
        //                 {
        //                     FullName = typeof(IntReadTunnel).FullName,
        //                     AssemblyLocation = typeof(IntReadTunnel).Assembly.Location
        //                 };
        //             case nameof(WriteStringTunnel):
        //                 return new ConfigurationEntry()
        //                 {
        //                     FullName = typeof(WriteStringTunnel).FullName,
        //                     AssemblyLocation = typeof(WriteStringTunnel).Assembly.Location
        //                 };
        //             default:
        //                 throw new ArgumentOutOfRangeException(nameof(contents));
        //         }
        //     }
        //
        //     public string Read(string pointer)
        //     {
        //         return pointer;
        //     }
        // }
    }
}