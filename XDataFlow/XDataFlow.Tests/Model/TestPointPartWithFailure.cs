using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Tests.Stubs;

namespace XDataFlow.Tests.Model
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