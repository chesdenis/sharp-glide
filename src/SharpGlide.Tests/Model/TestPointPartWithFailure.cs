using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tests.Model
{
    public class TestPointPartWithFailure : PointPart
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