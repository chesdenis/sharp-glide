using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Abstractions;
using SharpGlide.Tunnels.Write.Interfaces;

namespace SharpGlide.Tests.Behaviour.Flow.Concept.Examples
{
    public class StringSingleWriteTunnel : SingleWriteTunnel<string>
    {
        private readonly Func<List<string>> _storagePointer;
        
        public StringSingleWriteTunnel(Func<List<string>> storagePointer)
        {
            _storagePointer = storagePointer;
        }

        protected override async Task WriteImpl(string data, IRoute route, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().Add($"{data} -> {route}");
        }

        protected override async Task<string> WriteAndReturnImpl(string data, IRoute route,
            CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            _storagePointer().Add(data);

            return await Task.FromResult($"{data} - handled!");
        }
    }
}