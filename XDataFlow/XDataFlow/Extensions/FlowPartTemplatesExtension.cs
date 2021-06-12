using System;
using XDataFlow.Parts;
using XDataFlow.Tunnels;
using XDataFlow.Tunnels.InMemory;
using XDataFlow.Tunnels.InMemory.Messaging;

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