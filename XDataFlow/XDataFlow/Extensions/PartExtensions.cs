using System;
using XDataFlow.Parts;

namespace XDataFlow.Extensions
{
    public static class PartExtensions
    {
        public static void RegisterPart<T>(this PartsRegistry appRegistry, string partId, T partInstance) where T : IDataFlowPart
        {
            if (appRegistry.ContainsKey(partId))
            {
                throw new ArgumentException("This instance was added into dictionary already", partId);
            }
            
            appRegistry.Add(partId, partInstance);
        }
    }
}