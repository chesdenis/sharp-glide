using System;

namespace XDataFlow.Controllers.Consume
{
    public interface IConsumeMetrics
    {
        int GetWaitingToConsumeAmount();
        TimeSpan GetEstimatedTime();
        int GetMessagesPerSecond();
    }
}