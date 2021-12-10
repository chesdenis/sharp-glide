using System.Linq;
using SharpGlide.Providers;

namespace SharpGlide.Wrappers.Performance
{
    public class SpeedMetric
    {
        private readonly Metric[] _data;

        public SpeedMetric(Metric[] data)
        {
            _data = data;
        }
        
        public double CountLastMs => _data.Count(w => w.TimestampUtc >= DateTimeProvider.Now.AddMilliseconds(-1));
        public double CountLastSecond => _data.Count(w => w.TimestampUtc >= DateTimeProvider.Now.AddSeconds(-1));
        public double CountLastMinute => _data.Count(w => w.TimestampUtc >= DateTimeProvider.Now.AddMinutes(-1));
        public double CountLastHour => _data.Count(w => w.TimestampUtc >= DateTimeProvider.Now.AddHours(-1));
        public double CountLastDay => _data.Count(w => w.TimestampUtc >= DateTimeProvider.Now.AddDays(-1));

        public double AvgLastMs => _data
            .Where(w => w.TimestampUtc >= DateTimeProvider.Now.AddMilliseconds(-1))
            .Select(s => s.MetricValue).Average();
        public double AvgLastSecond => _data
            .Where(w => w.TimestampUtc >= DateTimeProvider.Now.AddSeconds(-1))
            .Select(s => s.MetricValue).Average();
        public double AvgLastMinute => _data
            .Where(w => w.TimestampUtc >= DateTimeProvider.Now.AddMinutes(-1))
            .Select(s => s.MetricValue).Average();
        public double AvgLastHour => _data
            .Where(w => w.TimestampUtc >= DateTimeProvider.Now.AddHours(-1))
            .Select(s => s.MetricValue).Average();
        public double AvgLastDay => _data
            .Where(w => w.TimestampUtc >= DateTimeProvider.Now.AddDays(-1))
            .Select(s => s.MetricValue).Average();
        
        // public override int EstimatedTimeInSeconds
        // {
        //     get
        //     {
        //         var currentWaitingToConsume = WaitingToConsume;
        //
        //         if (_previousWaitingToConsume > currentWaitingToConsume)
        //         {
        //             var processedMessagesDelta = Math.Abs(_previousWaitingToConsume - currentWaitingToConsume);
        //             if (processedMessagesDelta == 0)
        //             {
        //                 return 0;
        //             }
        //             
        //             var timeRangeDeltaInSeconds = DateTime.Now.Subtract(_previousWaitingToConsumeDateTime).TotalSeconds;
        //
        //             var secondsPerMessage = timeRangeDeltaInSeconds / processedMessagesDelta;
        //             var estimatedTime = Convert.ToInt32(secondsPerMessage * currentWaitingToConsume);
        //             _messagesPerSecond = Convert.ToInt32(timeRangeDeltaInSeconds > 0 ? processedMessagesDelta / timeRangeDeltaInSeconds : 0);
        //             
        //             return estimatedTime;
        //         }
        //         
        //         _previousWaitingToConsume = currentWaitingToConsume;
        //         _previousWaitingToConsumeDateTime = DateTime.Now;
        //
        //         return 0;
        //     }
        // }
    }
}