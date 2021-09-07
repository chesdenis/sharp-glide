using System;
using System.Threading;
using System.Threading.Tasks;
using DechFlow.Parts.Abstractions;
using DechFlow.Tests.Stubs;

namespace DechFlow.Tests.Model
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