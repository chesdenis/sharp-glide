using System;
using System.Collections.Concurrent;

namespace SharpGlide.Context.Abstractions
{
    public interface IMetaDataContext
    {
        string Name { get; set; }

        ConcurrentDictionary<string,string> Status { get; }
        
        ConcurrentBag<string> Errors { get; }
        
        void UpsertStatus(string key, string value);
        void ReportException(Exception ex);
    }
}