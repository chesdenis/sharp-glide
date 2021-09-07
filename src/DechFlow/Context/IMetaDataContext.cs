using System.Collections.Generic;

namespace DechFlow.Context
{
    public interface IMetaDataContext
    {
        string Name { get; set; }

        Dictionary<string,string> Status { get; }
        
        void UpsertStatus(string key, string value);
    }
}