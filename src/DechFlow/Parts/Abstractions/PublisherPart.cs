using DechFlow.Registry;

namespace DechFlow.Parts.Abstractions
{
    public abstract class PublisherPart<TPublishData> : VectorPart<Empty, TPublishData>
    {
        protected PublisherPart(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }
    }
}