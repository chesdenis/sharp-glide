using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tests.Model.PointPart
{
    public class TestPointPartWith1SecondExecution : SharpGlide.Parts.Abstractions.PointPart
    {
        public string TestProperty { get; set; }

        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

            TestProperty = "Completed";
        }
    }
}