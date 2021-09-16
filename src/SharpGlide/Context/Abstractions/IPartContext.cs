namespace SharpGlide.Context.Abstractions
{
    public interface IPartContext
    {
        IMetaDataContext MetaDataContext { get; }
        IGroupContext GroupContext { get; }
        IHeartBeatContext HeartBeatContext { get; }
        ISwitchContext SwitchContext { get; }
        ISettingsContext SettingsContext { get; }
    }
}