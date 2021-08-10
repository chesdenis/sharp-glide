using XDataFlow.Controllers.Consume;
using XDataFlow.Controllers.Group;
using XDataFlow.Controllers.MetaData;
using XDataFlow.Controllers.Metric;
using XDataFlow.Controllers.Switch;

namespace XDataFlow.Context
{
    public class PointPartContext : IPartContext
    {
        public IMetaDataController MetaDataController { get; }
        public IGroupController GroupController { get; }
        public IHeartBeatController HeartBeatController { get; }
        public IConsumeMetrics ConsumeMetrics { get; }
        public ISwitchController SwitchController { get; }

        public PointPartContext(
            IMetaDataController metaDataController, 
            IGroupController groupController, 
            IHeartBeatController heartBeatController, 
            IConsumeMetrics consumeMetrics, 
            ISwitchController switchController)
        {
            MetaDataController = metaDataController;
            GroupController = groupController;
            HeartBeatController = heartBeatController;
            ConsumeMetrics = consumeMetrics;
            SwitchController = switchController;
        }
 
    }
}