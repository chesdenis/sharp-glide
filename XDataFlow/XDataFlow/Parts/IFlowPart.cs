using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Parts
{
    public interface IFlowPart<TPublishData, TConsumeData> : IPart
    {
        void SetupFlow();
    }
}