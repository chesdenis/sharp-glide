using System;
using SharpGlide.Extensions;

namespace SharpGlide.Context.HeartBeat
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
        public override void UpdateStatus(int indentation = 0)
        {
            _metaDataContext.UpsertStatus("Name", _metaDataContext.Name.IndentLeft('-', indentation));
        }

        public override void UpdateStatus(string key, string value)
        {
            _metaDataContext.UpsertStatus(key, value);
        }

        public DateTime LastActivity { get; set; }
    }
}