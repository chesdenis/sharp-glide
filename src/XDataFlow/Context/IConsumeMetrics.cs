using System;

namespace XDataFlow.Context
{
    public interface IConsumeMetrics
    {
        int GetWaitingToConsumeAmount();
        TimeSpan GetEstimatedTime();
        int GetMessagesPerSecond();
    }
}