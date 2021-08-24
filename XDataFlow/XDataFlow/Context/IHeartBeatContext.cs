using System;

namespace XDataFlow.Context
{
    public interface IHeartBeatContext
    { 
        DateTime LastPublishedAt { get; set; }

        DateTime LastConsumedAt { get; set; }

        int IdleTimeoutMs { get; set; }

        bool Idle { get; }

        bool Failed { get; }

        void PrintStatusInfo();
        
        void PrintStatusInfo(string key, string value);
    }
}