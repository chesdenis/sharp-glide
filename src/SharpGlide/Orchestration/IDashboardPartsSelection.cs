using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Orchestration
{
    public interface IDashboardPartsSelection<TConsumeData, TPublishData>
    {
        IEnumerable<VectorPart<TConsumeData, TPublishData>> Selection { get; set; }
        IDashboard Dashboard { get; set; }
    }
}