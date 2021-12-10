using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tests.Model.PointPart
{
    public class TestPointPartWithGroupSupport : SharpGlide.Parts.Abstractions.PointPart
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