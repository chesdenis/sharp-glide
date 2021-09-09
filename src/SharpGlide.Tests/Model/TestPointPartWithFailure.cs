using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tests.Stubs;

namespace SharpGlide.Tests.Model
{
    public class TestPointPartWithFailure : PointPart
    {
        public TestPointPartWithFailure() : base(new DefaultRegistryStub())
        {
        }

        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);

            throw new Exception("Some Exception");
        }
    }
}