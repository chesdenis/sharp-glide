using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpGlide.Context;

namespace SharpGlide.Parts.Abstractions
{
    public interface IBasePart
    {
        string Name { get; set; }
        IPartContext Context { get; set; }
        ConcurrentDictionary<string, string> Status { get; }
        void ReportInfo(string status);
        void AddChild(IBasePart part);
        IBasePart GetChild(string name, bool recursive = false);
        void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false);
        Task StartAsync();
        Task StartAndStopAsync(TimeSpan onlinePeriod);
        Task StopAsync();
    }
}