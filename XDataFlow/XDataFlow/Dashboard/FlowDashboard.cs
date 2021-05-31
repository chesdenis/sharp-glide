using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Behaviours;
using XDataFlow.Extensions;
using XDataFlow.Parts;
using XDataFlow.Wrappers.Default;

namespace XDataFlow.Dashboard
{
    public class FlowDashboard : IFlowDashboard
    {
        private readonly IDictionary<Type, Func<IPart>> _partTemplates = new Dictionary<Type, Func<IPart>>();
      
        private readonly IDictionary<string, IPart> _parts  = new Dictionary<string, IPart>();
        
        public TFlowPart AddFlowPart<TFlowPart>(string name) where TFlowPart : IPart
        {
            var flowPart = CreateFlowPart<TFlowPart>();

            flowPart.Name = name;
            flowPart.Status.Upsert("Name", name);

            _parts.Add(name, flowPart);

            return flowPart;
        }

        public void WatchOnIdleParts(Action<IPart> onIdle, CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    foreach (var idlePart in _parts.Values.Where(w=>w.Idle).ToList())
                    {
                        onIdle(idlePart);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }
            }, cancellationToken);
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
        
        public IFlowDashboard AddFlowPartTemplate<TFlowPart>(Func<IPart> flowPartBuilder)
            where TFlowPart : IPart
        {
            _partTemplates.Add(typeof(TFlowPart), flowPartBuilder);
          
            return this;
        }

        public TFlowPart CreateFlowPart<TFlowPart>() where TFlowPart : IPart
        {
            return (TFlowPart) _partTemplates[typeof(TFlowPart)]();
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