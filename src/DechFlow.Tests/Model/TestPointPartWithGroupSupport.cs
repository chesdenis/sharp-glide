using System.Threading;
using System.Threading.Tasks;
using DechFlow.Parts.Abstractions;
using DechFlow.Tests.Stubs;

namespace DechFlow.Tests.Model
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