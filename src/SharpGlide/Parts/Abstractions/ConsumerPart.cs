namespace SharpGlide.Parts.Abstractions
{
    public abstract class ConsumerPart<TConsumeData> : VectorPart<TConsumeData, Empty>
    {
        protected ConsumerPart() : base()
        {
        }
    }
}