using System;

namespace SharpGlide.Context
{
    public interface IConsumeMetrics
    {
        int GetWaitingToConsumeAmount();
        TimeSpan GetEstimatedTime();
        int GetMessagesPerSecond();
    }
}