using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using XDataFlow.Behaviours;
using XDataFlow.Parts;

namespace XDataFlow.Dashboard
{
    public interface IFlowDashboard
    {
        IFlowDashboard AddFlowPartTemplate<TFlowPart>(Func<IPart> flowPartBuilder) where TFlowPart : IPart;

        TFlowPart CreateFlowPart<TFlowPart>() where TFlowPart : IPart;
        
        TFlowPart AddFlowPart<TFlowPart>(string name) where TFlowPart : IPart;

        void WatchOnIdleParts(Action<IPart> onIdle, CancellationToken cancellationToken);

        List<ExpandoObject> GetStatusInfo();

        IEnumerable<IPart> GetFlowParts();

        void EnumerateParts(Action<IPart> partAction);
    }
}