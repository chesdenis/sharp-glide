using System;

namespace XDataFlow.Refactored.Controllers.Consume
{
    public interface IConsumeMetrics
    {
        int GetWaitingToConsumeAmount();
        TimeSpan GetEstimatedTime();
        int GetMessagesPerSecond();
    }
}