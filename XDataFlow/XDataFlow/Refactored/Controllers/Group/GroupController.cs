using System;
using System.Collections.Generic;
using System.Linq;
using XDataFlow.Refactored;

namespace XDataFlow.Parts.Interfaces
{
    public class GroupController : IGroupController
    {
        public virtual void Configure()
        {
            
        }
        
        public IDictionary<string, IBasePart> Children { get; } = new Dictionary<string, IBasePart>();

        public TFlowPart AddFlowPart<TFlowPart>(TFlowPart flowPart, string name) where TFlowPart : IBasePart
        {
            flowPart.Name = name;

            Children.Add(name, flowPart);

            return flowPart;
        }
        
        public IEnumerable<IBasePart> GetChildren() => this.Children.Select(s => s.Value);

        
        public void EnumerateParts(Action<IBasePart> partAction)
        {
            foreach (var part in this.GetChildren())
            {
                partAction(part);
            }
        }
    }
}