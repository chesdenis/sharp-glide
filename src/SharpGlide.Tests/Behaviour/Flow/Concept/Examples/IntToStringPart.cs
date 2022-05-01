using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts;
using SharpGlide.Readers;
using SharpGlide.Routing;
using SharpGlide.Writers;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntToStringPart : IBasePart
    {
        private readonly ISingleReader<int> _readerIntReader;
        private readonly IWriter<string> _stringWriter;

        public string Name { get; set; }

        public IntToStringPart(
            ISingleReader<int> readerIntReader,
            IWriter<string> stringWriter)
        {
            _readerIntReader = readerIntReader;
            _stringWriter = stringWriter;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            var input = await this._readerIntReader.ReadAsync(cancellationToken);

            input *= 2;

            await _stringWriter.WriteSingle(input.ToString(), Route.Default, cancellationToken);
        }
    }
}