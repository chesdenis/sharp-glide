using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Context;
using SharpGlide.Context.HeartBeat;
using SharpGlide.Context.Switch;
using SharpGlide.Providers;

namespace SharpGlide.Parts.Abstractions
{
    public abstract class PointPart : BasePart
    {
        public abstract Task ProcessAsync(CancellationToken cancellationToken);

        protected PointPart()
        {
            var metaDataContext = new MetaDataContext();
            var groupContext = new GroupContext();

            var settingsContext = new SettingsContext();
            var switchContext = new PointPartSwitchContext(this);
            
            var heartBeatContext =
                new PointHeartBeatContext(groupContext, metaDataContext);

            Context = new PointPartContext(
                metaDataContext,
                groupContext,
                heartBeatContext,
                switchContext,
                settingsContext);
        }
    }
}