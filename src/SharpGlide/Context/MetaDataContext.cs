using System.Collections.Generic;

namespace SharpGlide.Context
{
    public class MetaDataContext : IMetaDataContext
    {
        public string Name { get; set; }
        public Dictionary<string, string> Status { get; } = new Dictionary<string, string>();
        
        public void UpsertStatus(string key, string value)
        {
            if (Status.ContainsKey(key))
            {
                Status[key] = value;
                return;
            }
            
            Status.Add(key, value);
        }
    }
}