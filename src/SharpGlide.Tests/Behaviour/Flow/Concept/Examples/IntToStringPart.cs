using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts;
using SharpGlide.Routing;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class ReadTunnel<T>
    {
        private readonly Func<CancellationToken, Task<T>> _readIntPointer;

        public ReadTunnel(Func<CancellationToken, Task<T>> readIntPointer)
        {
            _readIntPointer = readIntPointer;
        }

        public async Task<T> ReadAsync(CancellationToken token) => await _readIntPointer(token);
    }

    public class IntToStringPart : IBasePart
    {
        private readonly ReadTunnel<int> _readIntTunnel;
        private readonly Func<string, IRoute, CancellationToken, Task> _writeStringPointer;
        public string Name { get; set; }

        public IntToStringPart(
            ReadTunnel<int> readIntTunnel, 
            Func<string, IRoute, CancellationToken, Task> writeStringPointer)
        {
            _readIntTunnel = readIntTunnel;
            _writeStringPointer = writeStringPointer;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            var input = await _readIntTunnel.ReadAsync(cancellationToken);

            input *= 2;

            await _writeStringPointer(input.ToString(), Route.Default, cancellationToken);
        }
    }
}