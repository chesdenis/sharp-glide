using System;

namespace XDataFlow.Parts.Generic
{
    public class GenericGuard<TData> : FlowPart<TData, TData>
    {
        private readonly Func<TData, bool> _allowFunc;

        public GenericGuard(Func<TData, bool> allowFunc)
        {
            _allowFunc = allowFunc;
        }
         
        protected override void ProcessMessage(TData data)
        {
            if (_allowFunc(data))
            {
                this.Publish(data);
            }
        }
    }
}