using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tests.Model
{
    public class TestPointPartWith1SecondExecution : PointPart
    {
        public string TestProperty { get; set; }
        
        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

            TestProperty = "Completed";
        }
    }
}