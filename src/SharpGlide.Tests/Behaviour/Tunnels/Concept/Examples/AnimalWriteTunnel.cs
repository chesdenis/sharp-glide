using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Abstractions;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    public class AnimalWriteTunnel : WriteTunnel<AnimalWriteTunnel.IWritableAnimal>
    {
        private readonly List<IWritableAnimal> _storage;
        
        public AnimalWriteTunnel(List<IWritableAnimal> storage)
        {
            _storage = storage;
        }
        
        public interface IWritableAnimal
        {
            string Name { get; set; }
            string WritablePropertyA { get; set; }
        }
        
        protected override Task WriteSingleImpl(IWritableAnimal data, IRoute route, CancellationToken cancellationToken)
        {
            _storage.Add(data);

            return Task.CompletedTask;
        }

        protected override Task<IWritableAnimal> WriteAndReturnSingleImpl(IWritableAnimal data, IRoute route,
            CancellationToken cancellationToken)
        {
            _storage.Add(data);

            return Task.FromResult(data);
        }

        protected override Task WriteRangeImpl(IEnumerable<IWritableAnimal> dataRange, IRoute route,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IEnumerable<IWritableAnimal>> WriteAndReturnRangeImpl(IEnumerable<IWritableAnimal> dataRange,
            IRoute route, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}