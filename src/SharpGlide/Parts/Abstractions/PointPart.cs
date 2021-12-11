using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Context;

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