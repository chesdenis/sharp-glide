using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    public class AnimalWalkTunnel : WalkTunnel<AnimalWalkTunnel.IReadableAnimal>
    {
        private readonly List<IReadableAnimal> _storage;

        public AnimalWalkTunnel(List<IReadableAnimal> storage)
        {
            _storage = storage;
        }

        public interface IReadableAnimal
        {
            public string Name { get; set; }
            string WritablePropertyA { get; set; }
            string NonWritablePropertyB { get; set; }
        }

        protected override Task WalkImpl(CancellationToken cancellationToken,
            Action<IReadableAnimal> callback)
        {
            foreach (var animal in _storage)
            {
                callback(animal);
            }

            return Task.CompletedTask;
        }

        protected override async Task WalkAsyncImpl(CancellationToken cancellationToken,
            Func<CancellationToken, IReadableAnimal, Task> callback)
        {
            foreach (var animal in _storage)
            {
                await callback(cancellationToken, animal);
            }
        }


        protected override Task WalkPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Action<IEnumerable<IReadableAnimal>> callback)
        {
            callback(_storage);

            return Task.CompletedTask;
        }

        protected override async Task WalkPagedAsyncImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            Func<CancellationToken, IEnumerable<IReadableAnimal>, Task> callback)
        {
            await callback(cancellationToken, _storage);
        }
    }
}