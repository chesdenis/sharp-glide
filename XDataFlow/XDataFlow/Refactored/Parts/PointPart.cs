using System.Threading;
using XDataFlow.Parts.Interfaces;

namespace XDataFlow.Refactored
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

        public override ISwitchController SwitchController => new PointPartSwitchController(this);

        public abstract void Process(CancellationToken cancellationToken);
    }
}