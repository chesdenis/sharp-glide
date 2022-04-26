using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Readers.Proxy;
using SharpGlide.Tunnels.Writers.Proxy;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class IntToStringPart : IBasePart
    {
        private readonly IDirectReaderProxy<int> _directIntReader;
        private readonly IDirectWriterProxy<string> _directStringWriter;

        public string Name { get; set; }

        public IntToStringPart(
            IDirectReaderProxy<int> directIntReader,
            IDirectWriterProxy<string> directStringWriter)
        {
            _directIntReader = directIntReader;
            _directStringWriter = directStringWriter;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            var input = await this._directIntReader.ReadAsync(cancellationToken);

            input *= 2;

            await _directStringWriter.WriteSingle(input.ToString(), Route.Default, cancellationToken);
        }
    }
}