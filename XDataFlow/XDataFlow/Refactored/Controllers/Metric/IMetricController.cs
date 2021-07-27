using System;

namespace XDataFlow.Refactored.Controllers.Metric
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