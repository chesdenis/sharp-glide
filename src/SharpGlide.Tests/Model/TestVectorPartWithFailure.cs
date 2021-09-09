using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tests.Stubs;

namespace SharpGlide.Tests.Model
{
    public class TestVectorPartWithFailure : VectorPart<TestVectorPartWithFailure.Input, TestVectorPartWithFailure.Output>
    {
        public class Input
        {
                
        }
            
        public class Output
        {
                
        }

        public TestVectorPartWithFailure() : base(new DefaultRegistryStub())
        {
        }

        public override async Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);

            throw new Exception("Some Exception");
        }
    }
}