using XDataFlow.Registry;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class PublisherPart<TPublishData> : VectorPart<Empty, TPublishData>
    {
        protected PublisherPart(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }
    }
}