using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Orchestration
{
    public class DashboardSelection : IDashboardSelection
    {
        public IList<IBasePart> Selection { get; } = new List<IBasePart>();
    }
}