using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tests.Stubs;

namespace SharpGlide.Tests.Model
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