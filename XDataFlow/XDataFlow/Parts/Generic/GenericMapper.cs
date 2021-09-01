using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Extensions;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Parts.Generic
{
    public class GenericBuffer<TBufferData> : VectorPart<TBufferData, TBufferData[]>
    {
        private readonly List<TBufferData> _buffer = new List<TBufferData>();
        
        public override Task ProcessAsync(TBufferData data, CancellationToken cancellationToken)
        {
            // TODO
            
            return Task.CompletedTask;
        }
    }
    
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