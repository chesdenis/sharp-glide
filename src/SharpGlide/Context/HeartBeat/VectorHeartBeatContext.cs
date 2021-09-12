using System;
using SharpGlide.Extensions;
using SharpGlide.Providers;

namespace SharpGlide.Context.HeartBeat
{
    public class VectorHeartBeatContext : HeartBeatContext, IVectorHeartBeatContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IConsumeMetrics _consumeMetrics;
        private readonly IMetaDataContext _metaDataContext;

        public VectorHeartBeatContext(
            IDateTimeProvider dateTimeProvider,
            IConsumeMetrics consumeMetrics,
            IGroupContext groupContext,
            IMetaDataContext metaDataContext) : base(groupContext)
        {
            _dateTimeProvider = dateTimeProvider;
            _consumeMetrics = consumeMetrics;
            _metaDataContext = metaDataContext;

            LastPublishedAt = _dateTimeProvider.GetNow();
            LastConsumedAt = _dateTimeProvider.GetNow();
        }

        public DateTime LastPublishedAt { get; set; } 
        public DateTime LastConsumedAt { get; set; }
        public override int IdleTimeoutMs { get; set; }
        
        public override bool Idle => DateTime.Now.Subtract(LastPublishedAt).TotalMilliseconds > IdleTimeoutMs &
                            DateTime.Now.Subtract(LastConsumedAt).TotalMilliseconds > IdleTimeoutMs;

        public override bool Failed { get; }

        public override void UpdateStatus(int indentation = 0)
        {
            _metaDataContext.UpsertStatus("Name", _metaDataContext.Name.IndentLeft('-', indentation));
            _metaDataContext.UpsertStatus("Available", _consumeMetrics.GetWaitingToConsumeAmount().ToString());
            _metaDataContext.UpsertStatus("_ETA",_consumeMetrics.GetEstimatedTime().ToString("c"));
            _metaDataContext.UpsertStatus("_Speed, n/sec", _consumeMetrics.GetMessagesPerSecond().ToString());
        }

        public override void UpdateStatus(string key, string value)
        {
            _metaDataContext.UpsertStatus(key, value);
        }
    }
}