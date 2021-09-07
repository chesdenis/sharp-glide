using System.Threading;
using System.Threading.Tasks;
using DechFlow.Parts.Abstractions;
using DechFlow.Tests.Stubs;

namespace DechFlow.Tests.Model
{
    public class TestPointPart : PointPart
    {
        public TestPointPart() : base(new DefaultRegistryStub())
        {
        }

        public string TestProperty { get; set; }
            
        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            TestProperty = "ABCDE";

            await StopAsync();
        }
    }
}