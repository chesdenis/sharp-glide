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

        void Collect();
        
        void Collect(string key, string value);
        
        string ReportAsXml(IBasePart startPart);
    }
}