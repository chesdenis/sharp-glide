using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Orchestration
{
    public static class DashboardSelectionExtensions
    {
        public static IDashboardPartsSelection<TConsumeData, TPublishData> SelectParts<TConsumeData,
            TPublishData>(this IDashboard dashboard,
            params VectorPart<TConsumeData, TPublishData>[] parts)
        {
            return new DashboardPartsSelection<TConsumeData, TPublishData>
            {
                Selection = parts,
                Dashboard = dashboard
            };
        }
    }
}