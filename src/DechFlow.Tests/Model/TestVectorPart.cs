using System.Threading;
using System.Threading.Tasks;
using DechFlow.Parts.Abstractions;
using DechFlow.Tests.Stubs;

namespace DechFlow.Tests.Model
{
    public class TestVectorPart : VectorPart<TestVectorPart.Input, TestVectorPart.Output>
    {
        public class Input
        {
                
        }
            
        public class Output
        {
                
        }

        public TestVectorPart() : base(new DefaultRegistryStub())
        {
        }

        public string TestProperty { get; set; }

        public override async Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            TestProperty = "ABCDE";

            await StopAsync();
        }
    }
}