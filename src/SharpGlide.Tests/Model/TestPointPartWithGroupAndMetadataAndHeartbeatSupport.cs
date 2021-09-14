using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tests.Model
{
    public class TestPointPartWithGroupAndMetadataAndHeartbeatSupport : PointPart
    {
        public string TestProperty { get; set; }
        
        public TestPointPartWithGroupAndMetadataAndHeartbeatSupport() 
            : base()
        {
            
        }

        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            TestProperty = "Started";

            await StopAsync();
        }
    }
}