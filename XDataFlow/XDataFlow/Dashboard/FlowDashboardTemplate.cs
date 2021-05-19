using System;
using System.Collections.Generic;
using XDataFlow.Parts;

namespace XDataFlow.Dashboard
{
    public class FlowDashboardTemplate : IFlowDashboardTemplate
    {
        private readonly IDictionary<Type, Func<IPart>> _partTemplates  = new Dictionary<Type, Func<IPart>>();
        
        public IFlowDashboard Build()
        {
            return new FlowDashboard(this);
        }
        
        public IFlowDashboardTemplate AddFlowPartTemplate<TFlowPart>(Func<IPart> flowPartBuilder)
            where TFlowPart : IPart
        {
            _partTemplates.Add(typeof(TFlowPart), flowPartBuilder);
          
            return this;
        }

        public TFlowPart CreateFlowPart<TFlowPart>() where TFlowPart : IPart
        {
            return (TFlowPart) _partTemplates[typeof(TFlowPart)]();
        }
    }
}