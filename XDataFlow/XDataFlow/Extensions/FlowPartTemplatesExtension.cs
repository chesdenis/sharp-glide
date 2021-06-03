using System;
using XDataFlow.Parts;

namespace XDataFlow.Extensions
{
    public static class FlowPartTemplatesExtension
    {
        public static TFlowPart CreateFlowPart<TFlowPart>(this Func<TFlowPart> flowPartTemplate) where TFlowPart : IPart
        {
            return flowPartTemplate();
        }
    }
}