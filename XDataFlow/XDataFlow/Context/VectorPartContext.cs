using XDataFlow.Controllers.Consume;
using XDataFlow.Controllers.Group;
using XDataFlow.Controllers.MetaData;
using XDataFlow.Controllers.Metric;
using XDataFlow.Controllers.Publish;
using XDataFlow.Controllers.Switch;

namespace XDataFlow.Context
{
    public class VectorPartContext<TConsumeData, TPublishData> : PointPartContext
    {
        public IPublishController<TPublishData> PublishController { get; }
        public IConsumeController<TConsumeData> ConsumeController { get; }

        public VectorPartContext(
            IMetaDataController metaDataController, 
            IGroupController groupController, 
            IHeartBeatController heartBeatController,
            IConsumeMetrics consumeMetrics, 
            ISwitchController switchController,
            IConsumeController<TConsumeData> consumeController,
            IPublishController<TPublishData> publishController) 
            : base(metaDataController, groupController, heartBeatController, consumeMetrics, switchController)
        {
            PublishController = publishController;
            ConsumeController = consumeController;
        }
    }
}