namespace XDataFlow.Refactored
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