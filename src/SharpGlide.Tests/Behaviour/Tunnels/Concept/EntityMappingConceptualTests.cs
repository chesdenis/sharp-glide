using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Flow;
using SharpGlide.Readers;
using SharpGlide.Routing;
using SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples;
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
            var writableAnimals = storage.OfType<AnimalWriteTunnel.IWritableAnimal>().ToList();
            var animalWriteTunnel = new AnimalWriteTunnel(writableAnimals);

            var walker = model.BuildWalker(animalReadTunnel) as ISingleWalker<AnimalWalkTunnel.IReadableAnimal>;
            var writer = model.BuildWriter(animalWriteTunnel);

            // Act
            await writer.WriteSingle(animal1, Route.Default, CancellationToken.None);
            await writer.WriteSingle(animal2, Route.Default, CancellationToken.None);
            await writer.WriteSingle(animal3, Route.Default, CancellationToken.None);

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
            var writableAnimals = storage.OfType<AnimalWriteTunnel.IWritableAnimal>().ToList();
            var animalWriteTunnel = new AnimalWriteTunnel(writableAnimals);

            var walker = model.BuildWalker(animalReadTunnel);
            var writer = model.BuildWriter(animalWriteTunnel);

            var part = new AnimalProcessingPart(walker, writer);

            // Act
            await part.ProcessAsync(CancellationToken.None);

            // Assert
            writableAnimals.Count.Should().Be(6);
        }

        public class Animal :
            AnimalWalkTunnel.IReadableAnimal,
            AnimalWriteTunnel.IWritableAnimal
        {
            public string Name { get; set; }
            public string WritablePropertyA { get; set; }
            public string NonWritablePropertyB { get; set; }
        }
    }
}