using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Tests.Stubs;

namespace XDataFlow.Tests.Model
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
            this.TestProperty = "ABCDE";

            await this.StopAsync();
        }
    }
}