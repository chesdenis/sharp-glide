using System;
using XDataFlow.Extensions;

namespace XDataFlow.Parts.Generic
{
    public class GenericMapper<TConsumeData, TPublishData> : FlowPart<TConsumeData, TPublishData>
    {
        private readonly Func<TConsumeData, TPublishData> _mapFunc;

        public GenericMapper(Func<TConsumeData, TPublishData> mapFunc)
        {
            _mapFunc = mapFunc;
        }
         
        protected override void ProcessMessage(TConsumeData data)
        {
            var transformed = _mapFunc(data);

            this.Publish<GenericMapper<TConsumeData, TPublishData>, TPublishData>(transformed);
        }
    }
}