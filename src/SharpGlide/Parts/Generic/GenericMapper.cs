using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Parts.Generic
{
    public class GenericMapper<TConsumeData, TPublishData> : VectorPart<TConsumeData, TPublishData>
    {
        private readonly Func<TConsumeData, TPublishData> _mapFunc;

        public GenericMapper(Func<TConsumeData, TPublishData> mapFunc) : base()
        {
            _mapFunc = mapFunc;
        }

        public override Task ProcessAsync(TConsumeData data, CancellationToken cancellationToken)
        {
            var transformed = _mapFunc(data);
    
            Publish(transformed);

            return Task.CompletedTask;
        }
    }
}