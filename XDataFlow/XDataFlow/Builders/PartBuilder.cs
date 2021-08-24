using System;
using System.Collections.Generic;
using XDataFlow.Context;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Builders
{
    public class PartBuilder : IPartBuilder
    {
        private readonly Func<IMetaDataContext> _metaDataFunc;
        private readonly Func<IGroupContext> _groupFunc;
        private readonly Func<IHeartBeatContext> _heartBeatFunc;
        private readonly Func<IConsumeMetrics> _consumeMetricsFunc;

        public PartBuilder(
            Func<IMetaDataContext> metaDataFunc,
            Func<IGroupContext> groupFunc,
            Func<IHeartBeatContext> heartBeatFunc,
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
                new PointPartSwitchContext(part));

            return part;
        }
        

        public T CreateVectorPart<T, TConsumeData, TPublishData>(
            Func<VectorPart<TConsumeData, TPublishData>> partFunc = null) where T : VectorPart<TConsumeData, TPublishData>
        {
            var part = partFunc?.Invoke() ?? Activator.CreateInstance<T>();
            
            var heartBeatContext = _heartBeatFunc();
            
            _consumeContextBag.TryGetValue(typeof(TConsumeData), out var consumeContextFunc);
            var typedConsumeContextFunc =
                consumeContextFunc as Func<IConsumeContext<TConsumeData>>
                ??
                (() => new ConsumeContext<TConsumeData>());
            
            _publishContextBag.TryGetValue(typeof(TPublishData), out var publishContext);
           
            var typedPublishContextFunc =
                publishContext as Func<IPublishContext<TPublishData>>
                ??
                (() => new PublishContext<TConsumeData, TPublishData>(heartBeatContext));
            
            var typedConsumeContext = typedConsumeContextFunc();
            var typedPublishContext = typedPublishContextFunc();
            
            part.Context = new VectorPartContext<TConsumeData, TPublishData>(
                _metaDataFunc(),
                _groupFunc(),
                heartBeatContext,
                _consumeMetricsFunc(),
                new VectorPartSwitchContext<TConsumeData, TPublishData>(part, typedConsumeContext),
                typedConsumeContext,
                typedPublishContext);
            
            return (T) part;
        }

        private readonly Dictionary<Type, Func<IConsumeContext<object>>> _consumeContextBag =
            new Dictionary<Type, Func<IConsumeContext<object>>>();
        
        private readonly Dictionary<Type, Func<IConsumeContext<object>>> _publishContextBag =
            new Dictionary<Type, Func<IConsumeContext<object>>>();

        public void SetConsumeContextFunc<TConsumeData>(Func<IConsumeContext<TConsumeData>> consumeContextFunc)
        {
            _consumeContextBag.Add(typeof(TConsumeData), (Func<IConsumeContext<object>>) consumeContextFunc);
        }

        public void SetPublishContextFunc<TPublishData>(Func<IPublishContext<TPublishData>> publishContextFunc)
        {
            _publishContextBag.Add(typeof(TPublishData), (Func<IConsumeContext<object>>) publishContextFunc);
        }
    }
}