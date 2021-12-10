using System.Collections.Generic;
using System.Dynamic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Context.Abstractions
{
    public interface IHeartBeatContext
    {
        int IdleTimeoutMs { get; set; }

        bool Idle { get; }

        bool Failed { get; }

        void UpdateStatus();
        
        void UpdateStatus(string key, string value);
        
        List<ExpandoObject> GetStatus(IBasePart startPart);

        string GetException(IBasePart startPart);
        
        string GetStatusTable(IBasePart startPart);
        string GetExceptionList(IBasePart startPart);
    }
}