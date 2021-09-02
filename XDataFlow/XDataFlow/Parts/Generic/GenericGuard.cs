using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;

namespace XDataFlow.Parts.Generic
{
    public abstract class GenericGuard<TConsumeData> : VectorPart<TConsumeData, TConsumeData>
    {
        protected GenericGuard(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }

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