using System.Collections.Generic;
using System.Linq;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tests.Model.VectorPart
{
    public abstract class TestVectorPartAssertable<TConsumeData, TPublishData> : VectorPart<TConsumeData, TPublishData>
    {
        protected TestVectorPartAssertable() : base()
        {
        }

        public List<TConsumeData> ConsumedData { get; } = new();
        public List<TPublishData> PublishedData { get; } = new();

        public bool WasConsumed(TConsumeData data)
        {
            return Enumerable.Contains(ConsumedData, data);
        }

        public bool WasPublished(TPublishData data)
        {
            return Enumerable.Contains(PublishedData, data);
        }
            
        public abstract TPublishData Map(TConsumeData data);
    }
}