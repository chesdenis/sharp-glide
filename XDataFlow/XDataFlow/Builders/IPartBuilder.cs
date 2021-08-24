using System;
using XDataFlow.Context;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Builders
{
    public interface IPartBuilder
    {
        T CreatePointPart<T>(Func<T> partFunc = null) where T : PointPart;

        T CreateVectorPart<T, TConsumeData, TPublishData>(
            Func<VectorPart<TConsumeData, TPublishData>> partFunc = null) 
            where T: VectorPart<TConsumeData, TPublishData>;

        void SetConsumeContextFunc<TConsumeData>(Func<IConsumeContext<TConsumeData>> consumeContextFunc);

        void SetPublishContextFunc<TPublishData>(Func<IPublishContext<TPublishData>> publishContextFunc);
    }
}