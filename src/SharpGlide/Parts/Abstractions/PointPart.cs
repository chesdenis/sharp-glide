using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Context;
using SharpGlide.Context.HeartBeat;
using SharpGlide.Context.Switch;
using SharpGlide.Providers;
using SharpGlide.Registry;

namespace SharpGlide.Parts.Abstractions
{
    public abstract class PointPart : BasePart
    {
        public abstract Task ProcessAsync(CancellationToken cancellationToken);

        protected PointPart(IDefaultRegistry defaultRegistry)
        {
            var metaDataContext = defaultRegistry.Get<IMetaDataContext>() ?? throw new ArgumentNullException(nameof(IMetaDataContext));
            var groupContext = defaultRegistry.Get<IGroupContext>() ?? throw new ArgumentNullException(nameof(IGroupContext));

            var settingsContext = defaultRegistry.Get<ISettingsContext>() ??
                                  throw new ArgumentNullException(nameof(ISettingsContext));
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