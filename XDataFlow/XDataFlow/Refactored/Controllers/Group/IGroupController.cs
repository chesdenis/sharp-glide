using System;
using System.Collections.Generic;
using XDataFlow.Refactored.Parts;

namespace XDataFlow.Refactored.Controllers.Group
{
    public interface IGroupController
    {
        TFlowPart AddFlowPart<TFlowPart>(TFlowPart flowPart, string name) where TFlowPart : IBasePart;
        IEnumerable<IBasePart> GetChildren();
        
        void EnumerateParts(Action<IBasePart> partAction);
        
        IDictionary<string, IBasePart> Children { get; }
        void Configure();
    }
}