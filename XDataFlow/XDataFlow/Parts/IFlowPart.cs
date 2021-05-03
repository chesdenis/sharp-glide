using System.Collections.Generic;

namespace XDataFlow.Parts
{
    public interface IFlowPart<TPublishData, TConsumeData> : IRestartablePart
    {
        IDictionary<string, IFlowPart<TPublishData, TConsumeData>> Children { get; }
    }
}