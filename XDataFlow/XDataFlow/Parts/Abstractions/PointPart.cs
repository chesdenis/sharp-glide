using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Builders;
using XDataFlow.Context;
using XDataFlow.Registry;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class PointPart : BasePart
    {
        public abstract Task ProcessAsync(CancellationToken cancellationToken);

        protected PointPart()
        {
            var metaDataContext = XFlowDefaultRegistry.Get<IMetaDataContext>() ?? throw new ArgumentNullException(nameof(IMetaDataContext));
            var groupContext = XFlowDefaultRegistry.Get<IGroupContext>() ?? throw new ArgumentNullException(nameof(IGroupContext)); 
            var heartBeatContext = XFlowDefaultRegistry.Get<IHeartBeatContext>() ?? throw new ArgumentNullException(nameof(IHeartBeatContext));
            var consumeMetrics = XFlowDefaultRegistry.Get<IConsumeMetrics>() ?? throw new ArgumentNullException(nameof(IConsumeMetrics));

            var switchContext = new PointPartSwitchContext(this);
            
            this.Context = new PointPartContext(
                metaDataContext,
                groupContext,
                heartBeatContext,
                switchContext);
        }
        
        public static PointPart CreateUsing(IPartBuilder partBuilder = null)
        {
            if (partBuilder == null)
            {
                throw new NotImplementedException("Create default part builder as singleton");
            }
            
            return partBuilder.CreatePointPart<PointPart>();
        }
    }
}