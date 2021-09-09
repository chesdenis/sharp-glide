using SharpGlide.Registry;

namespace SharpGlide.Parts.Abstractions
{
    public abstract class PublisherPart<TPublishData> : VectorPart<Empty, TPublishData>
    {
        protected PublisherPart(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }
    }
}