using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
{
    public abstract class PipelinePart<TConsumeData, TPublishData> : BasePart
    {
        private readonly IGroupController _groupController;
        private readonly IConsumeController<TConsumeData> _consumeController;

        protected PipelinePart(
            IMetaDataController metaDataController,
            IGroupController groupController,
            IConsumeController<TConsumeData> consumeController
        ) : base(metaDataController)
        {
            _groupController = groupController;
            _consumeController = consumeController;
        }
        
        private ISwitchController _switchController;
        public override ISwitchController SwitchController
        {
            get
            {
                _switchController ??= new PipelinePartSwitchController<TConsumeData, TPublishData>(this, _consumeController);
                return _switchController;
            }
        }

        public abstract Task ProcessAsync(
            TConsumeData consumeData, 
            CancellationToken cancellationToken);
    }
}