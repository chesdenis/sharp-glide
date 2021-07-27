using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
{
    public abstract class PointPart : BasePart
    {
        private readonly IMetaDataController _metaDataController;
        
        private readonly IGroupController _groupController;

        protected PointPart(
            IMetaDataController metaDataController,
            IGroupController groupController
            ) : base(metaDataController)
        {
            _metaDataController = metaDataController;
            _groupController = groupController;
        }

        private ISwitchController _switchController;
        public override ISwitchController SwitchController
        {
            get
            {
                _switchController ??= new PointPartSwitchController(this);
                return _switchController;
            }
        }

        public abstract Task ProcessAsync(CancellationToken cancellationToken);
    }
}