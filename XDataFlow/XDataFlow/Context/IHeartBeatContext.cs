using System;
using System.Collections.Generic;
using System.Dynamic;

namespace XDataFlow.Context
{
    public interface IHeartBeatContext
    { 
        DateTime LastPublishedAt { get; set; }

        DateTime LastConsumedAt { get; set; }

        int IdleTimeoutMs { get; set; }

        bool Idle { get; }

        bool Failed { get; }

        void UpdateStatus();
        
        void UpdateStatus(string key, string value);
        List<ExpandoObject> GetStatus();
    }
}