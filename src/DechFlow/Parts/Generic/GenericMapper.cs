using System;
using System.Threading;
using System.Threading.Tasks;
using DechFlow.Parts.Abstractions;
using DechFlow.Registry;

namespace DechFlow.Parts.Generic
{
    public class GenericMapper<TConsumeData, TPublishData> : VectorPart<TConsumeData, TPublishData>
    {
        private readonly Func<TConsumeData, TPublishData> _mapFunc;

        public GenericMapper(Func<TConsumeData, TPublishData> mapFunc, IDefaultRegistry registry) : base(registry)
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