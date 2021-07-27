using XDataFlow.Parts;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;

namespace XDataFlow.Refactored.Parts
{
    public abstract class PublisherPart<TPublishData> : PipelinePart<Empty, TPublishData>
    {
        protected PublisherPart(
            IMetaDataController metaDataController, 
            IGroupController groupController,
            IConsumeController<Empty> consumeController) 
            : base(metaDataController, groupController, consumeController)
        {
        }
    }
}