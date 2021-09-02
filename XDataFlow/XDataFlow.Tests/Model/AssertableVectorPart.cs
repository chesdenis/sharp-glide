using System.Collections.Generic;
using System.Linq;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;
using XDataFlow.Tests.Stubs;

namespace XDataFlow.Tests.Model
{
    public abstract class AssertableVectorPart<TConsumeData, TPublishData> : VectorPart<TConsumeData, TPublishData>
    {
        protected AssertableVectorPart() : base(new DefaultRegistryStub())
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