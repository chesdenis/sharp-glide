using System.Collections.Generic;

namespace XDataFlow.Extensions
{
    public static class UpsertExtensions
    {
        public static void Upsert(this Dictionary<string,string> data, string key, string value)
        {
            if (data.ContainsKey(key))
            {
                data[key] = value;
                return;
            }
            
            data.Add(key, value);
        }
    }
}