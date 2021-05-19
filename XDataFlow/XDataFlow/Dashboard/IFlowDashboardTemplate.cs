using System;
using XDataFlow.Parts;

namespace XDataFlow.Dashboard
{
    public interface IFlowDashboardTemplate
    {
        IFlowDashboard Build();

        IFlowDashboardTemplate AddFlowPartTemplate<TFlowPart>(Func<IPart> flowPartBuilder) where TFlowPart : IPart;

        TFlowPart CreateFlowPart<TFlowPart>() where TFlowPart : IPart;
    }
}