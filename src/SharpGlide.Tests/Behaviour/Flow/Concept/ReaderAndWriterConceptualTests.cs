using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.FlowSchema;
using SharpGlide.Routing;
using SharpGlide.Tests.Behaviour.Flow.Concept.Examples;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Flow.Concept
{
    public class IntToStringFlowTests
    {
        public static List<int> SourceDataA = Enumerable.Range(2, 10000).ToList();

        [Fact]
        public async Task IntToStringFlowShouldCalculateStrings()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();

            flowModel.Tunnels.Add($"reader", new IntReader(() => SourceDataA));
            flowModel.Tunnels.Add($"writer", new StringWriter(() => output));

            var reader = flowModel.Tunnels["reader"] as IntReader;
            var writer = flowModel.Tunnels["writer"] as StringWriter;

            var part = new IntToStringPart(
                new ReadTunnel<int>(
                    reader.ReadSingleExpr.Compile()
                    ), 
                writer.WriteSingleExpr.Compile());

            flowModel.Parts.Add("int2string", part);

            // Act
            await part.ProcessAsync(CancellationToken.None);

            // Assert
            output.Should().HaveCount(1);
            output.First().Should().StartWith("4");
        }
    }

    public class ReaderAndWriterConceptualTests
    {
        public static List<int> SourceDataA = Enumerable.Range(0, 10000).ToList();
        public static List<int> SourceDataB = Enumerable.Range(0, 10000).ToList();
        public static List<int> SourceDataC = Enumerable.Range(0, 10000).ToList();

        [Fact]
        public async Task OnDemandReaderShouldGetSingleEntry()
        {
            // Arrange
            var flowModel = new Model();

            flowModel.Tunnels.Add($"{nameof(SourceDataA)}Reader", new IntReader(() => SourceDataA));

            var intReader = flowModel.Tunnels[$"{nameof(SourceDataA)}Reader"] as IntReader;

            // Act
            var readSingle = await intReader.ReadSingleExpr.Compile()(CancellationToken.None);

            // Assert
            readSingle.Should().Be(0);
        }

        [Fact]
        public async Task OnDemandReaderShouldGetRangeEntries()
        {
            // Arrange
            var flowModel = new Model();

            flowModel.Tunnels.Add($"{nameof(SourceDataA)}Reader", new IntReader(() => SourceDataA));

            var intReader = flowModel.Tunnels[$"{nameof(SourceDataA)}Reader"] as IntReader;

            // Act
            var readRangeFunc = intReader.ReadRangeExpr.Compile();
            var readRange = await readRangeFunc(CancellationToken.None,
                (ints => ints.Skip(10).Take(10)));

            // Assert
            readRange.Count().Should().Be(10);
            readRange.First().Should().Be(10);
            readRange.Last().Should().Be(19);
        }

        [Fact]
        public async Task OnDemandReaderShouldGetAllEntries()
        {
            // Arrange
            var flowModel = new Model();

            flowModel.Tunnels.Add($"{nameof(SourceDataA)}Reader", new IntReader(() => SourceDataA));

            var intReader = flowModel.Tunnels[$"{nameof(SourceDataA)}Reader"] as IntReader;

            // Act
            var readAll = await intReader.ReadAllExpr.Compile()(CancellationToken.None);

            // Assert
            readAll.Count().Should().Be(10000);
        }

        [Fact]
        public async Task OnDemandWriterShouldWriteSingleEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();

            flowModel.Tunnels.Add($"StringWriterA", new StringWriter(() => output));

            var stringWriter = flowModel.Tunnels["StringWriterA"] as StringWriter;

            // Act
            var writeSingleFunc = stringWriter.WriteSingleExpr.Compile();

            // Assert
            await writeSingleFunc("test1", Route.Default, CancellationToken.None);

            output.First().Should().StartWith("test1");
        }

        [Fact]
        public async Task OnDemandWriterShouldWriteAndReturnSingleEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();

            flowModel.Tunnels.Add($"StringWriterA", new StringWriter(() => output));

            var stringWriter = flowModel.Tunnels["StringWriterA"] as StringWriter;

            // Act
            var writeFunc = stringWriter.WriteAndReturnSingleExpr.Compile();

            // Assert
            var retVal = await writeFunc("test1", Route.Default, CancellationToken.None);

            output.First().Should().StartWith("test1");
            retVal.Should().Contain("handled");
        }

        [Fact]
        public async Task OnDemandWriterShouldWriteRangeEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();

            flowModel.Tunnels.Add($"StringWriterA", new StringWriter(() => output));

            var stringWriter = flowModel.Tunnels["StringWriterA"] as StringWriter;

            // Act
            var writeRangeFunc = stringWriter.WriteRangeExpr.Compile();

            // Assert
            await writeRangeFunc(new List<string>() { "a", "b", "c" }, Route.Default, CancellationToken.None);

            output.Count.Should().Be(3);
        }

        [Fact]
        public async Task OnDemandWriterShouldWriteAndReturnRangeEntry()
        {
            // Arrange
            var flowModel = new Model();
            var output = new List<string>();

            flowModel.Tunnels.Add($"StringWriterA", new StringWriter(() => output));

            var stringWriter = flowModel.Tunnels["StringWriterA"] as StringWriter;

            // Act
            var writeRangeFunc = stringWriter.WriteAndReturnRangeExpr.Compile();

            // Assert
            var result = await writeRangeFunc(new List<string>() { "a", "b", "c" }, Route.Default,
                CancellationToken.None);

            result.Count().Should().Be(3);
        }
    }
}