using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tests.Model.VectorPart
{
    public class TestVectorPartMultiThreadWithExecutionJob : MultiThreadVectorPart<int, string>
    {
        public override int GetApproxMaxThreads() => 10;

        public readonly ConcurrentBag<int> PushedData = new ConcurrentBag<int>();
        
        public override async Task MultiThreadProcessAsync(int data, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            
            PushedData.Add(data);
        }
    }
}