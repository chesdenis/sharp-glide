using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Readers;
using SharpGlide.Readers.Interfaces;
using SharpGlide.Routing;
using SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples;
using SharpGlide.Tunnels.Write.Interfaces;
using SharpGlide.Writers;
using SharpGlide.Writers.Interfaces;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept
{
    public class EntityMappingConceptualTests
    {
        [Fact]
        public async Task ShouldSupportTunnelCastBasedOnInterface()
        {
            // Arrange
            var animal1 = new Animal();
            var animal2 = new Animal();
            var animal3 = new Animal();

            var storage = new List<Animal>() { animal1, animal2, animal3 };

            var model = new FlowModel();
            
            var readableAnimals = storage.OfType<AnimalWalkTunnel.IReadableAnimal>().ToList();
            var animalReadTunnel = new AnimalWalkTunnel(readableAnimals);
            var writableAnimals = storage.OfType<AnimalSingleWriteTunnel.IWritableAnimal>().ToList();
            var animalWriteTunnel = new AnimalSingleWriteTunnel(writableAnimals);

            var walker = model.BuildWalker(animalReadTunnel) as ISingleWalker<AnimalWalkTunnel.IReadableAnimal>;
            var writer = model.BuildSingleWriter(animalWriteTunnel) as ISingleWriter<AnimalSingleWriteTunnel.IWritableAnimal>;

            // Act
            await writer.Write(animal1, Route.Default, CancellationToken.None);
            await writer.Write(animal2, Route.Default, CancellationToken.None);
            await writer.Write(animal3, Route.Default, CancellationToken.None);

            var walkerData = new List<AnimalWalkTunnel.IReadableAnimal>();
            await walker.WalkAsync(CancellationToken.None,
                (data => { walkerData.Add(data); }));

            // Assert
            walkerData.Count.Should().Be(3);
            writableAnimals.Count.Should().Be(6);
        }

        [Fact]
        public async Task ShouldSupportCastInsidePart()
        {
            // Arrange
            var animal1 = new Animal();
            var animal2 = new Animal();
            var animal3 = new Animal();

            var storage = new List<Animal>() { animal1, animal2, animal3 };

            var model = new FlowModel();

            //var animalReadTunnel = new AnimalReadTunnel(storage);
            var readableAnimals = storage.OfType<AnimalWalkTunnel.IReadableAnimal>().ToList();
            var animalReadTunnel = new AnimalWalkTunnel(readableAnimals);
            var writableAnimals = storage.OfType<AnimalSingleWriteTunnel.IWritableAnimal>().ToList();
            var animalWriteTunnel = new AnimalSingleWriteTunnel(writableAnimals);

            var walker = model.BuildWalker(animalReadTunnel);
            var writer = model.BuildSingleWriter(animalWriteTunnel);

            var part = new AnimalProcessingPart(walker, writer);

            // Act
            await part.ProcessAsync(CancellationToken.None);

            // Assert
            writableAnimals.Count.Should().Be(6);
        }

        public class Animal :
            AnimalWalkTunnel.IReadableAnimal,
            AnimalSingleWriteTunnel.IWritableAnimal
        {
            public string Name { get; set; }
            public string WritablePropertyA { get; set; }
            public string NonWritablePropertyB { get; set; }
        }
    }
}