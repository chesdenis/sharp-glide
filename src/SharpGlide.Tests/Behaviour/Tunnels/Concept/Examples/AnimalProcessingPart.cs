using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts;
using SharpGlide.Readers;
using SharpGlide.Readers.Interfaces;
using SharpGlide.Routing;
using SharpGlide.Writers;
using SharpGlide.Writers.Interfaces;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    public class AnimalProcessingPart : IBasePart
    {
        private readonly ISingleAsyncWalker<AnimalWalkTunnel.IReadableAnimal> _animalWalker;
        private readonly ISingleWriter<AnimalSingleWriteTunnel.IWritableAnimal> _animalWriter;

        public string Name { get; set; }

        public AnimalProcessingPart(
            ISingleAsyncWalker<AnimalWalkTunnel.IReadableAnimal> animalWalker,
            ISingleWriter<AnimalSingleWriteTunnel.IWritableAnimal> animalWriter)
        {
            _animalWalker = animalWalker;
            _animalWriter = animalWriter;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            await _animalWalker.WalkAsyncSingleAsync(cancellationToken, OneReceive);
        }

        private async Task OneReceive(CancellationToken cancellationToken,
            AnimalWalkTunnel.IReadableAnimal readableAnimal)
        {
            var animal = (AnimalSingleWriteTunnel.IWritableAnimal)readableAnimal;

            await this._animalWriter.WriteSingle(animal, Route.Default, cancellationToken);
        }
    }
}