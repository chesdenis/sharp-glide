using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Abstractions;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    public class AnimalSingleWriteTunnel : SingleWriteTunnel<AnimalSingleWriteTunnel.IWritableAnimal>
    {
        private readonly List<IWritableAnimal> _storage;
        
        public AnimalSingleWriteTunnel(List<IWritableAnimal> storage)
        {
            _storage = storage;
        }
        
        public interface IWritableAnimal
        {
            string Name { get; set; }
            string WritablePropertyA { get; set; }
        }
        
        protected override Task WriteImpl(IWritableAnimal data, IRoute route, CancellationToken cancellationToken)
        {
            _storage.Add(data);

            return Task.CompletedTask;
        }

        protected override Task<IWritableAnimal> WriteAndReturnImpl(IWritableAnimal data, IRoute route,
            CancellationToken cancellationToken)
        {
            _storage.Add(data);

            return Task.FromResult(data);
        }
    }
}