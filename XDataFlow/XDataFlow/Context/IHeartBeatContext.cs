using System;
using System.Collections.Generic;
using System.Dynamic;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Context
{
    public interface IHeartBeatContext
    { 
        DateTime LastPublishedAt { get; set; }

        DateTime LastConsumedAt { get; set; }

        int IdleTimeoutMs { get; set; }

        bool Idle { get; }

        bool Failed { get; }

        void UpdateStatus(int indentation = 0);
        
        void UpdateStatus(string key, string value);
        List<ExpandoObject> GetStatus(IBasePart startPart);
        string GetStatusTable(IBasePart startPart);
    }
}