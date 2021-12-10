using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tests.Model.PointPart
{
    public class TestPointPartWithFailure : SharpGlide.Parts.Abstractions.PointPart
    {
        public TestPointPartWithFailure() : base()
        {
        }

        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);

            throw new Exception("Some Exception");
        }
    }
}