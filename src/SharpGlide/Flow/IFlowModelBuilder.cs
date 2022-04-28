using System.Collections.Generic;

namespace SharpGlide.Flow
{
    public interface IFlowModelBuilder
    {
        FlowModel Configure<T>(
            IEnumerable<string> partsFiles,
            IEnumerable<string> tunnelsFiles,
            IEnumerable<string> settingsFiles) where T : IFlowModelEntryProvider, new();
    }
}