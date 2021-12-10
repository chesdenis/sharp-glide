using System;
using SharpGlide.Context.Abstractions;
using SharpGlide.Extensions;

namespace SharpGlide.Context
{
    public class PointHeartBeatContext : HeartBeatContext, IPointHeartBeatContext
    {
        private readonly IMetaDataContext _metaDataContext;

        public PointHeartBeatContext(IGroupContext groupContext, IMetaDataContext metaDataContext) : base(groupContext)
        {
            _metaDataContext = metaDataContext;
        }

        public override int IdleTimeoutMs { get; set; }
        public override bool Idle { get; }
        public override bool Failed { get; }
        public override void UpdateStatus()
        {
            _metaDataContext.UpsertStatus("Name", _metaDataContext.Name);
            _metaDataContext.UpsertStatus("IdleTimeoutMs", IdleTimeoutMs.ToString());
            _metaDataContext.UpsertStatus("Idle", Idle.ToString());
        }

        public override void UpdateStatus(string key, string value)
        {
            _metaDataContext.UpsertStatus(key, value);
        }

        public DateTime LastActivity { get; set; }
    }
}