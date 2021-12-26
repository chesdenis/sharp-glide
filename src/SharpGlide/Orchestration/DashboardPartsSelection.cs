using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Orchestration
{
    public class DashboardPartsSelection<TConsumeData, TPublishData> : IDashboardPartsSelection<TConsumeData, TPublishData>
    {
        public IEnumerable<VectorPart<TConsumeData, TPublishData>> Selection { get; set; }
        public IDashboard Dashboard { get; set; }
    }
}