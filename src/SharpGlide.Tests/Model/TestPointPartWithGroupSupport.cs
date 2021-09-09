using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tests.Stubs;

namespace SharpGlide.Tests.Model
{
    public class TestPointPartWithGroupSupport : PointPart
    {
        public string TestProperty { get; set; }

        public TestPointPartWithGroupSupport() : base(new RegistryStubWithGroupAndMetadataContext())
        {
            
        } 

        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            TestProperty = "Started";

            await StopAsync();
        }
    }
}