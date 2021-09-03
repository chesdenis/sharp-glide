using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Tests.Stubs;

namespace XDataFlow.Tests.Model
{
    public class TestPointPartWithGroupSupport : PointPart
    {
        public TestPointPartWithGroupSupport() : base(new RegistryStubWithGroupAndMetadataContext())
        {
            
        } 

        public override Task ProcessAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}