using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Orchestration
{
    public interface IDashboardSelection
    {
        IList<IBasePart> Selection { get; }
    }
}