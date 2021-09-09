using System.Collections.Generic;

namespace SharpGlide.Context
{
    public interface IMetaDataContext
    {
        string Name { get; set; }

        Dictionary<string,string> Status { get; }
        
        void UpsertStatus(string key, string value);
    }
}