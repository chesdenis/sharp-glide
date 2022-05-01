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
 
        protected override Task SingleWalkImpl(CancellationToken cancellationToken, Action<IReadableAnimal> callback)
        {
            foreach (var animal in _storage)
            {
                callback(animal);
            }
            
            return Task.CompletedTask;
        }

        protected override async Task SingleAsyncWalkImpl(CancellationToken cancellationToken, Func<CancellationToken, IReadableAnimal, Task> callback)
        {
            foreach (var animal in _storage)
            {
                await callback(cancellationToken, animal);
            }
        }

        protected override Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, Action<IEnumerable<IReadableAnimal>> callback)
        {
            callback(_storage);
            
            return Task.CompletedTask;
        }

        protected override async Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, Func<CancellationToken, IEnumerable<IReadableAnimal>, Task> callback)
        {
            await callback(cancellationToken, _storage);
        }
    }
}