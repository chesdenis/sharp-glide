using XDataFlow.Registry;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class ConsumerPart<TConsumeData> : VectorPart<TConsumeData, Empty>
    {
        protected ConsumerPart(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }
    }
}