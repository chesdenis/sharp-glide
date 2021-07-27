using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
{
    public abstract class BasePart : IBasePart
    {
        private readonly IMetaDataController _metaDataController;

        protected BasePart(IMetaDataController metaDataController)
        {
            _metaDataController = metaDataController;
        }
        public string Name
        {
            get => _metaDataController.Name;
            set => _metaDataController.Name = value;
        }

        public abstract ISwitchController SwitchController { get; }
    }
}