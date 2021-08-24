namespace XDataFlow.Context
{
    public interface IPartContext
    {
        IMetaDataContext MetaDataContext { get; }
        IGroupContext GroupContext { get; }
        IHeartBeatContext HeartBeatContext { get; }
        IConsumeMetrics ConsumeMetrics { get; }
        ISwitchContext SwitchContext { get; }
    }
}