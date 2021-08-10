using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Metric;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
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