using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts;
using SharpGlide.Readers;
using SharpGlide.Readers.Interfaces;
using SharpGlide.Routing;
using SharpGlide.Writers;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    public class AnimalProcessingPart : IBasePart
    {
        private readonly ISingleAsyncWalker<AnimalWalkTunnel.IReadableAnimal> _animalWalker;
        private readonly IWriter<AnimalWriteTunnel.IWritableAnimal> _animalWriter;

        public string Name { get; set; }

        public AnimalProcessingPart(
            ISingleAsyncWalker<AnimalWalkTunnel.IReadableAnimal> animalWalker,
            IWriter<AnimalWriteTunnel.IWritableAnimal> animalWriter)
        {
            _animalWalker = animalWalker;
            _animalWriter = animalWriter;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            await _animalWalker.WalkAsync(cancellationToken, OneReceive);
        }

        private async Task OneReceive(CancellationToken cancellationToken,
            AnimalWalkTunnel.IReadableAnimal readableAnimal)
        {
            var animal = (AnimalWriteTunnel.IWritableAnimal)readableAnimal;

            await this._animalWriter.WriteSingle(animal, Route.Default, cancellationToken);
        }
    }
}