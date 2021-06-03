using System;
using XDataFlow.Extensions;

namespace XDataFlow.Parts.Generic
{
    public class GenericDecorator<TDecorateData, TInput, TOutput> : FlowPart<TDecorateData, TDecorateData>
    {
        private readonly Func<TInput> _inputPointer;
        private readonly Action<TOutput> _assignActionPointer;
        private readonly Action<Func<TInput>, Action<TOutput>> _decorateActions;

        public GenericDecorator(
            Func<TInput> inputPointer,
            Action<TOutput> assignActionPointer, Action<Func<TInput>, Action<TOutput>> decorateActions)
        {
            _inputPointer = inputPointer;
            _assignActionPointer = assignActionPointer;
            _decorateActions = decorateActions;
        }
        
        protected override void ProcessMessage(TDecorateData data)
        {
            _decorateActions(_inputPointer, _assignActionPointer);
            
            this.Publish(data);
        }
    }

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