namespace XDataFlow.Wrappers
{
    public interface IPerformanceMetrics
    {
        double ItemsForLastSecond { get; }

        double ItemsForLastMinute { get; }

        double ItemsForLastHour { get; }

        double AvgProcessingTimeInSecondsForLastMinute { get; }
    }
}