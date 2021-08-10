using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Metric;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
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