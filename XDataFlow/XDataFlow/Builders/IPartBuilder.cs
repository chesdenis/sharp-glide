using System;
using XDataFlow.Controllers.Consume;
using XDataFlow.Controllers.Publish;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Builders
{
    public interface IPartBuilder
    {
        T CreatePointPart<T>(Func<T> partFunc = null) where T : PointPart;
        
        T CreateVectorPart<T, TConsumeData, TPublishData>(
            Func<IConsumeController<TConsumeData>> consumeControllerFunc,
            Func<IPublishController<TPublishData>> publishControllerFunc,
            Func<T> partFunc = null) 
            where T : VectorPart<TConsumeData, TPublishData>;
    }
}