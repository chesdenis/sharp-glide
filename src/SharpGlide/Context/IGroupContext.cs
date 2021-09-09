using System;
using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Context
{
    public interface IGroupContext
    {
        IEnumerable<Tuple<int, IBasePart>> GetPartTree(IBasePart parentPart, int parentLevel = 0);

        void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false);
        
        IDictionary<string, IBasePart> Children { get; }

        void AddChild(IBasePart child);
        
        IBasePart GetChild(string name, bool recursive = false);
    }
}