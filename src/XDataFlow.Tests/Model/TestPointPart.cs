using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Tests.Stubs;

namespace XDataFlow.Tests.Model
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