using System;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Publish;

namespace XDataFlow.Refactored.Parts
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