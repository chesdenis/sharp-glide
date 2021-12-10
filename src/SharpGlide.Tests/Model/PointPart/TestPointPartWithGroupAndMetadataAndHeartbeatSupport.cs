using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Tests.Model.PointPart
{
    public class TestPointPartWithGroupAndMetadataAndHeartbeatSupport : SharpGlide.Parts.Abstractions.PointPart
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