using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tests.Model
{
    public class TestMultiThreadVectorPartWithExecutionJob : MultiThreadVectorPart<int, string>
    {
        public override int GetApproxMaxThreads() => 10;

        public readonly List<int> PushedData = new List<int>();
        
        public override async Task MultiThreadProcessAsync(int data, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            
            PushedData.Add(data);
        }
    }
}