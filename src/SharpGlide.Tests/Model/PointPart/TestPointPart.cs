using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tests.Model.PointPart
{
    public class TestPointPart : SharpGlide.Parts.Abstractions.PointPart
    {
        public TestPointPart() : base()
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