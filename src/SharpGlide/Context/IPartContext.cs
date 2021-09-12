using SharpGlide.Context.HeartBeat;
using SharpGlide.Context.Switch;

namespace SharpGlide.Context
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