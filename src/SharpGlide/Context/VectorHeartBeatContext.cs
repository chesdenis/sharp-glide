using System;
using SharpGlide.Context.Abstractions;
using SharpGlide.Providers;

namespace SharpGlide.Context
{
    public class VectorHeartBeatContext : HeartBeatContext, IVectorHeartBeatContext
    {
        private readonly IMetaDataContext _metaDataContext;

        public VectorHeartBeatContext(
            IGroupContext groupContext,
            IMetaDataContext metaDataContext) : base(groupContext)
        {
            _metaDataContext = metaDataContext;

            LastPublishedAt = DateTimeProvider.Now;
            LastConsumedAt = DateTimeProvider.Now;
        }

        public DateTime LastPublishedAt { get; set; }
        public DateTime LastConsumedAt { get; set; }
        
        public int Buffer { get; set; }
        public override int IdleTimeoutMs { get; set; }

        public override bool Idle => DateTimeProvider.Now.Subtract(LastPublishedAt).TotalMilliseconds > IdleTimeoutMs &
                                     DateTimeProvider.Now.Subtract(LastConsumedAt).TotalMilliseconds > IdleTimeoutMs;

        public override bool Failed { get; }

        public override void Collect()
        {
            _metaDataContext.UpsertStatus("Name", _metaDataContext.Name);
            _metaDataContext.UpsertStatus("LastPublishedAt", LastPublishedAt.ToString("s"));
            _metaDataContext.UpsertStatus("LastConsumedAt", LastConsumedAt.ToString("s"));
            _metaDataContext.UpsertStatus("IdleTimeoutMs", IdleTimeoutMs.ToString());
            _metaDataContext.UpsertStatus("Idle", Idle.ToString());
        }
    }
}