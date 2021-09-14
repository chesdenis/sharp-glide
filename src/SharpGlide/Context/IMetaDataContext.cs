using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SharpGlide.Context
{
    public interface IMetaDataContext
    {
        string Name { get; set; }

        ConcurrentDictionary<string,string> Status { get; }
        
        void UpsertStatus(string key, string value);
    }
}