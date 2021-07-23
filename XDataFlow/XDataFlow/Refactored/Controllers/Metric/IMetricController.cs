using System;

namespace XDataFlow.Parts.Interfaces
{
    public interface IMetricController<TConsumeData, IPublishData>
    { 
        DateTime LastPublishedAt { get; set; }

        DateTime LastConsumedAt { get; set; }

        int IdleTimeoutMs { get; set; }

        bool Idle { get; }

        void PrintStatusInfo();
    }
}