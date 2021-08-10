using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Extensions;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Parts.Generic
{
    public class GenericMapper<TConsumeData, TPublishData> : VectorPart<TConsumeData, TPublishData>
    {
        private readonly Func<TConsumeData, TPublishData> _mapFunc;
    
        public GenericMapper(Func<TConsumeData, TPublishData> mapFunc)
        {
            _mapFunc = mapFunc;
        }
          
        public override Task ProcessAsync(TConsumeData data, CancellationToken cancellationToken)
        {
            var transformed = _mapFunc(data);
    
            this.Publish(transformed);

            return Task.CompletedTask;
        }
    }
}