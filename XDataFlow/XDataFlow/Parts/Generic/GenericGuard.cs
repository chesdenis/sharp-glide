using System;

namespace XDataFlow.Parts.Generic
{
    public abstract class GenericGuard<TData> : FlowPart<TData, TData>
    {
        protected abstract bool IsAllow(TData data);

        protected override void ProcessMessage(TData data)
        {
            if (IsAllow(data))
            {
                this.Publish(data);
            }
        }
    }
}