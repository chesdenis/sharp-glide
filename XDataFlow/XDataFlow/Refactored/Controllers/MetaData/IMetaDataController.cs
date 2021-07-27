using System.Collections.Generic;

namespace XDataFlow.Refactored.Controllers.MetaData
{
    public interface IMetaDataController
    {
        string Name { get; set; }

        Dictionary<string,string> Status { get; }
        
        void UpsertStatus(string key, string value);
    }
}