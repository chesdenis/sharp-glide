using System;
using System.Threading;
using System.Threading.Tasks;
using DechFlow.Context;
using DechFlow.Registry;

namespace DechFlow.Parts.Abstractions
{
    public abstract class PointPart : BasePart
    {
        public abstract Task ProcessAsync(CancellationToken cancellationToken);

        protected PointPart(IDefaultRegistry defaultRegistry)
        {
            var metaDataContext = defaultRegistry.Get<IMetaDataContext>() ?? throw new ArgumentNullException(nameof(IMetaDataContext));
            var groupContext = defaultRegistry.Get<IGroupContext>() ?? throw new ArgumentNullException(nameof(IGroupContext)); 
            var heartBeatContext = defaultRegistry.Get<IHeartBeatContext>() ?? throw new ArgumentNullException(nameof(IHeartBeatContext));
            var consumeMetrics = defaultRegistry.Get<IConsumeMetrics>() ?? throw new ArgumentNullException(nameof(IConsumeMetrics));

            var settingsContext = defaultRegistry.Get<ISettingsContext>() ??
                                  throw new ArgumentNullException(nameof(ISettingsContext));
            var switchContext = new PointPartSwitchContext(this);
            
            Context = new PointPartContext(
                metaDataContext,
                groupContext,
                heartBeatContext,
                switchContext,
                settingsContext);
        }
    }
}