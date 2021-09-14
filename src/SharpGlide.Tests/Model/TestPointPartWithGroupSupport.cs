using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tests.Model
{
    public class TestPointPartWithGroupSupport : PointPart
    {
        public string TestProperty { get; set; }

        public TestPointPartWithGroupSupport() : base()
        {
            
        } 

        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            TestProperty = "Started";

            await StopAsync();
        }
    }
}