using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SharpGlide.Context
{
    public class MetaDataContext : IMetaDataContext
    {
        public string Name { get; set; }
        public ConcurrentDictionary<string, string> Status { get; } = new ConcurrentDictionary<string, string>();
        
        public void UpsertStatus(string key, string value)
        {
            if (Status.ContainsKey(key))
            {
                Status.TryUpdate(key, value, Status[key]);
                return;
            }
            
            Status.TryAdd(key, value);
        }
    }
}