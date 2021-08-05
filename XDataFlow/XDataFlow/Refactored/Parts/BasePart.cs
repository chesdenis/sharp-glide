using System;
using System.ComponentModel;
using System.Threading.Tasks;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Metric;
using XDataFlow.Refactored.Controllers.Switch;
using XDataFlow.Refactored.Controllers.Switch.Behaviours;

namespace XDataFlow.Refactored.Parts
{
    public interface IPartContext
    {
        IMetaDataController MetaData { get; }
        IGroupController Group { get; }
        IHeartBeatController HeartBeat { get; }
        IConsumeMetrics ConsumeMetrics { get; }
        ISwitchController Switch { get; }
    }
    
    public class PointPartContext : IPartContext
    {
        public IMetaDataController MetaData { get; }
        public IGroupController Group { get; }
        public IHeartBeatController HeartBeat { get; }
        public IConsumeMetrics ConsumeMetrics { get; }
        public ISwitchController Switch { get; }

        public PointPartContext(
            IMetaDataController metaData, 
            IGroupController @group, 
            IHeartBeatController heartBeat, 
            IConsumeMetrics consumeMetrics, 
            ISwitchController @switch)
        {
            MetaData = metaData;
            Group = @group;
            HeartBeat = heartBeat;
            ConsumeMetrics = consumeMetrics;
            Switch = @switch;
        }
 
    }

    public class VectorPartContext<TConsumeData, TPublishData> : PointPartContext
    {
        public IConsumeController<TConsumeData> Consume { get; }

        public VectorPartContext(
            IMetaDataController metaData, 
            IGroupController @group, 
            IHeartBeatController heartBeat,
            IConsumeMetrics consumeMetrics, 
            ISwitchController @switch,
            IConsumeController<TConsumeData> consume) 
            : base(metaData, @group, heartBeat, consumeMetrics, @switch)
        {
            Consume = consume;
        }
    }

    public abstract class BasePart : IBasePart
    {
        public IPartContext Context { get; set; }

        public string Name
        {
            get => Context.MetaData.Name;
            set => Context.MetaData.Name = value;
        }

        public void ConfigureStartAs<TBehaviour>(Func<TBehaviour> behaviourFunc)
            where TBehaviour : IStartBehaviour
        {
            Context.Switch.StartBehaviour = behaviourFunc();
        }

        public void ConfigureStartAs<TBehaviour>()
            where TBehaviour : IStartBehaviour, new()
        {
            Context.Switch.StartBehaviour = new TBehaviour();
        }

        public void ConfigureStopAs<TBehaviour>(TBehaviour behaviour)
            where TBehaviour : IStopBehaviour
        {
            Context.Switch.StopBehaviour = behaviour;
        }

        public void ConfigureStopAs<TBehaviour>()
            where TBehaviour : IStopBehaviour, new()
        {
            Context.Switch.StopBehaviour = new TBehaviour();
        }

        public async Task StartAsync()
        {
            await Context.Switch.StartAsync();
        }

        public async Task StopAsync()
        {
            await Context.Switch.StopAsync();
        }
    }

    public interface IPartBuilder
    {
        T CreatePointPart<T>(Func<T> partFunc = null) where T : PointPart;
        T CreateVectorPart<T, TConsumeData, TPublishData>(Func<IConsumeController<TConsumeData>> consumeControllerFunc, Func<T> partFunc = null) where T : VectorPart<TConsumeData, TPublishData>;
    }
    
    public class PartBuilder : IPartBuilder
    {
        private readonly Func<IMetaDataController> _metaDataFunc;
        private readonly Func<IGroupController> _groupFunc;
        private readonly Func<IHeartBeatController> _heartBeatFunc;
        private readonly Func<IConsumeMetrics> _consumeMetricsFunc;

        public PartBuilder(
            Func<IMetaDataController> metaDataFunc,
            Func<IGroupController> groupFunc,
            Func<IHeartBeatController> heartBeatFunc,
            Func<IConsumeMetrics> consumeMetricsFunc
            )
        {
            _metaDataFunc = metaDataFunc;
            _groupFunc = groupFunc;
            _heartBeatFunc = heartBeatFunc;
            _consumeMetricsFunc = consumeMetricsFunc;
        }

        public T CreatePointPart<T>(Func<T> partFunc = null) where T : PointPart
        {
            var part = partFunc?.Invoke() ?? Activator.CreateInstance<T>();
            
            part.Context = new PointPartContext(
                _metaDataFunc(), 
                _groupFunc(), 
                _heartBeatFunc(), 
                _consumeMetricsFunc(),
                new PointPartSwitchController(part));

            return part;
        }

        public T CreateVectorPart<T, TConsumeData, TPublishData>(Func<IConsumeController<TConsumeData>> consumeControllerFunc, Func<T> partFunc = null) where T : VectorPart<TConsumeData, TPublishData>
        {
            var part = partFunc?.Invoke() ?? Activator.CreateInstance<T>();

            var consumeController = consumeControllerFunc();

            part.Context = new VectorPartContext<TConsumeData, TPublishData>(
                _metaDataFunc(),
                _groupFunc(),
                _heartBeatFunc(),
                _consumeMetricsFunc(),
                new VectorPartSwitchController<TConsumeData, TPublishData>(part, consumeController), consumeController);

            return part;
        }
    }
}