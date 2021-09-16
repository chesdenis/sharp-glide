using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpGlide.Context;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Parts.Abstractions
{
    public interface IBasePart
    {
        string Name { get; set; }
        IPartContext Context { get; set; }
        ConcurrentDictionary<string, string> Status { get; }
        ConcurrentBag<string> Errors { get; }
        void ReportInfo(string status);
        void Report(string key, string value);
        void ReportThreads(int threadsAmount);
        IBasePart AddChild(IBasePart part);
        IBasePart GetChild(string name, bool recursive = false);
        void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false);
        Task StartAsync();
        Task StartAndStopAsync(TimeSpan onlinePeriod);
        Task StopAsync();

        string GetExceptionList();
        void ReportException(Exception ex);
    }
}