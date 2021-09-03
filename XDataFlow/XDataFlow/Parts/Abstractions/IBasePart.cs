using System;
using System.Collections.Generic;
using XDataFlow.Context;

namespace XDataFlow.Parts.Abstractions
{
    public interface IBasePart
    {
        string Name { get; set; }
        IPartContext Context { get; set; }
        Dictionary<string, string> Status { get; }
        void PrintStatus(string status);
        void AddChild(IBasePart part);
        IBasePart GetChild(string name, bool recursive = false);
        void EnumerateParts(Action<IBasePart> partAction, bool recursive);
    }
}