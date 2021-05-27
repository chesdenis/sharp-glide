using System;
using System.Collections.Generic;
using System.Dynamic;
using XDataFlow.Behaviours;
using XDataFlow.Parts;

namespace XDataFlow.Dashboard
{
    public interface IFlowDashboard
    {
        IFlowDashboard AddFlowPartTemplate<TFlowPart>(Func<IPart> flowPartBuilder) where TFlowPart : IPart;

        TFlowPart CreateFlowPart<TFlowPart>() where TFlowPart : IPart;
        
        TFlowPart AddFlowPart<TFlowPart>(string name) where TFlowPart : IPart;

        List<ExpandoObject> GetStatusInfo();

        IEnumerable<IPart> GetFlowParts();

        void EnumerateParts(Action<IPart> partAction);
    }
}