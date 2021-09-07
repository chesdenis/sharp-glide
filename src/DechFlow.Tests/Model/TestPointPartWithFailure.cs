using System;
using System.Threading;
using System.Threading.Tasks;
using DechFlow.Parts.Abstractions;
using DechFlow.Tests.Stubs;

namespace DechFlow.Tests.Model
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