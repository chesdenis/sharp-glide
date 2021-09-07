using System;

namespace DechFlow.Context
{
    public interface IConsumeMetrics
    {
        int GetWaitingToConsumeAmount();
        TimeSpan GetEstimatedTime();
        int GetMessagesPerSecond();
    }
}