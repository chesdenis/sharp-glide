using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Parts.Generic
{
    public abstract class GenericGuard<TConsumeData> : VectorPart<TConsumeData, TConsumeData>
    {
        protected abstract bool IsAllow(TConsumeData data);
    
        public override Task ProcessAsync(
            TConsumeData data, 
            CancellationToken cancellationToken)
        {
            if (IsAllow(data))
            {
                this.Publish(data);
            }

            return Task.CompletedTask;
        }
    }
}