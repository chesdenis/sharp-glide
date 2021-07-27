using XDataFlow.Parts;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;

namespace XDataFlow.Refactored.Parts
{
    public abstract class ConsumerPart<TConsumeData> : PipelinePart<TConsumeData, Empty>
    {
        protected ConsumerPart(
            IMetaDataController metaDataController, 
            IGroupController groupController,
            IConsumeController<TConsumeData> consumeController) 
            : base(metaDataController, groupController, consumeController)
        {
        }
    }
}