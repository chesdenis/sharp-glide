using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using XDataFlow.Behaviours;
using XDataFlow.Extensions;
using XDataFlow.Parts;
using XDataFlow.Wrappers.Default;

namespace XDataFlow.Dashboard
{
    public class FlowDashboard : IFlowDashboard
    {
        private readonly FlowDashboardTemplate _flowDashboardTemplate;
        private readonly IDictionary<string, IPart> _parts  = new Dictionary<string, IPart>();

        public FlowDashboard(FlowDashboardTemplate flowDashboardTemplate)
        {
            _flowDashboardTemplate = flowDashboardTemplate;
        }
        
        public TFlowPart AddFlowPart<TFlowPart>(string name) where TFlowPart : IPart
        {
            var flowPart = _flowDashboardTemplate.CreateFlowPart<TFlowPart>();

            flowPart.Name = name;
            flowPart.Status.Upsert("Name", name);

            _parts.Add(name, flowPart);

            return flowPart;
        }
  
        public List<ExpandoObject> GetStatusInfo()
        {
            foreach (var partsKey in _parts.Keys)
            {
                var part = _parts[partsKey];
                part.CollectStatusInfo();
            }
            
            var partsStatusKeys = _parts
                .Select(s => s.Value)
                .SelectMany(ss => ss.Status.Keys)
                .Distinct()
                .OrderBy(o => o)
                .ToList();

            var dataToPlot = new List<ExpandoObject>();

            foreach (var partsKey in _parts.Keys)
            {
                var part = _parts[partsKey];

                var partStatus = new ExpandoObject();
                var partStatusAsDict = (IDictionary<string, object>) partStatus;

                foreach (var partsStatusKey in partsStatusKeys)
                {
                    partStatusAsDict[partsStatusKey] =
                        part.Status.ContainsKey(partsStatusKey) == true ? part.Status[partsStatusKey] : "";
                }

                dataToPlot.Add(partStatus);
            }

            return dataToPlot;
        }

        public IEnumerable<IPart> GetFlowParts()
        {
            return this._parts.Select(s => s.Value);
        }

        public void EnumerateParts(Action<IPart> partAction)
        {
            foreach (var part in this.GetFlowParts())
            {
                partAction(part);
            }
        }
    }
}