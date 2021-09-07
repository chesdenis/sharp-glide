using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DechFlow.Context;

namespace DechFlow.Parts.Abstractions
{
    public interface IBasePart
    {
        string Name { get; set; }
        IPartContext Context { get; set; }
        Dictionary<string, string> Status { get; }
        void ReportInfo(string status);
        void AddChild(IBasePart part);
        IBasePart GetChild(string name, bool recursive = false);
        void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false);
        Task StartAsync();
        Task StartAndStopAsync(TimeSpan onlinePeriod);
        Task StopAsync();
    }
}