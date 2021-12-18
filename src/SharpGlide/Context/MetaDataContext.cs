using System;
using System.Collections.Concurrent;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Context
{
    public class MetaDataContext : IMetaDataContext
    {
        public string Name { get; set; }
        public ConcurrentDictionary<string, string> Status { get; } = new ConcurrentDictionary<string, string>();
        public ConcurrentBag<string> Exceptions { get; } = new ConcurrentBag<string>();

        public void UpsertStatus(string key, string value)
        {
            if (Status.ContainsKey(key))
            {
                Status.TryUpdate(key, value, Status[key]);
                return;
            }
            
            Status.TryAdd(key, value);
        }

        public void ReportException(Exception ex)
        {
            Exceptions.Add($"Failure of {Name}: {ex}");
        }
    }
}