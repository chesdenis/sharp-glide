using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Parts.Abstractions
{
    public interface IBasePart
    {
        string Name { get; set; }
        IPartContext Context { get; set; }
        void ReportInfo(string status);
        void Report(string key, string value);
        void ReportThreads(int threadsAmount);
        IBasePart AddChild(IBasePart part);
        IBasePart GetChild(string name, bool recursive = false);
        void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false);
        Task StartAsync();
        Task StartAndStopAsync(TimeSpan onlinePeriod);
        Task StopAsync();
        
        void ReportException(Exception ex);
    }
}