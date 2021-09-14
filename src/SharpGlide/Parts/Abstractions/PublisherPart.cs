namespace SharpGlide.Parts.Abstractions
{
    public abstract class PublisherPart<TPublishData> : VectorPart<Empty, TPublishData>
    {
        protected PublisherPart() : base()
        {
        }
    }
}