using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Metric;
using XDataFlow.Refactored.Controllers.Publish;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
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