using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tests.Model.VectorPart
{
    public class TestVectorPartWithFailure : VectorPart<TestVectorPartWithFailure.Input, TestVectorPartWithFailure.Output>
    {
        public class Input
        {
                
        }
            
        public class Output
        {
                
        }

        public TestVectorPartWithFailure() : base()
        {
        }

        public override async Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);

            throw new Exception("Some Exception");
        }
    }
}