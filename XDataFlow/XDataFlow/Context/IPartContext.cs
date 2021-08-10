using XDataFlow.Controllers.Consume;
using XDataFlow.Controllers.Group;
using XDataFlow.Controllers.MetaData;
using XDataFlow.Controllers.Metric;
using XDataFlow.Controllers.Switch;

namespace XDataFlow.Context
{
    public interface IPartContext
    {
        IMetaDataController MetaDataController { get; }
        IGroupController GroupController { get; }
        IHeartBeatController HeartBeatController { get; }
        IConsumeMetrics ConsumeMetrics { get; }
        ISwitchController SwitchController { get; }
    }
}