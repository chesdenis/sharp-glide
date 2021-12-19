using System.Linq;
using SharpGlide.Context.Abstractions;
using SharpGlide.Model;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Context
{
    public abstract class HeartBeatContext : IHeartBeatContext
    {
        private readonly IGroupContext _groupContext;
        private readonly IMetaDataContext _metaDataContext;

        protected HeartBeatContext(
            IGroupContext groupContext,
            IMetaDataContext metaDataContext)
        {
            _groupContext = groupContext;
            _metaDataContext = metaDataContext;
        }

        public abstract int IdleTimeoutMs { get; set; }
        public abstract bool Idle { get; }
        public abstract bool Failed { get; }

        public IPublishTunnel<HeartBeat> HeartBeatTunnel { get; set; }

        public abstract void Collect();

        public HeartBeat Get()
        {
            Collect();

            return new HeartBeat
            {
                Level = 0,
                Children = _groupContext.Children
                    .Select(s => s.Value.Context.HeartBeatContext.Get())
                    .ToArray(),
                Data = _metaDataContext.Status.Select(ss => new HeartBeatEntry()
                {
                    Name = ss.Key,
                    Value = ss.Value
                }).ToArray(),
                Exceptions = _metaDataContext.Exceptions.ToArray()
            };
        }
    }
}