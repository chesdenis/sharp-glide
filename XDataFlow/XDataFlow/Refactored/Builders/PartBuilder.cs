using System;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Controllers.Metric;
using XDataFlow.Refactored.Controllers.Publish;
using XDataFlow.Refactored.Controllers.Switch;

namespace XDataFlow.Refactored.Parts
{
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

        public T CreateVectorPart<T, TConsumeData, TPublishData>(
            Func<IConsumeController<TConsumeData>> consumeControllerFunc,
            Func<IPublishController<TPublishData>> publishControllerFunc,
            Func<T> partFunc = null) 
            where T : VectorPart<TConsumeData, TPublishData>
        {
            var part = partFunc?.Invoke() ?? Activator.CreateInstance<T>();

            var consumeController = consumeControllerFunc();
            var publishController = publishControllerFunc();

            part.Context = new VectorPartContext<TConsumeData, TPublishData>(
                _metaDataFunc(),
                _groupFunc(),
                _heartBeatFunc(),
                _consumeMetricsFunc(),
                new VectorPartSwitchController<TConsumeData, TPublishData>(part, consumeController),
                consumeController,
                publishController);

            return part;
        }
    }
}