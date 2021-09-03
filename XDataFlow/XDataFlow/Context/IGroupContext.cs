using System;
using System.Collections.Generic;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Context
{
    public interface IGroupContext
    {
        IEnumerable<Tuple<int, IBasePart>> GetChildrenTree(int parentLevel = 0);

        void EnumerateParts(Action<IBasePart> partAction, bool recursive);
        
        IDictionary<string, IBasePart> Children { get; }
        
        void AddChild(IBasePart part);
        IBasePart GetChild(string name, bool recursive = false);
    }
}