using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Parts.Generic
{
    public abstract class GenericGuard<TConsumeData> : VectorPart<TConsumeData, TConsumeData>
    {
        protected GenericGuard() : base()
        {
        }

        protected abstract bool IsAllow(TConsumeData data);
    
        public override Task ProcessAsync(
            TConsumeData data, 
            CancellationToken cancellationToken)
        {
            if (IsAllow(data))
            {
                Publish(data);
            }

            return Task.CompletedTask;
        }
    }
}