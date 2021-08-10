using System;
using System.Collections.Generic;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Controllers.Group
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