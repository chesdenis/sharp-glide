using System.Collections.Generic;

namespace SharpGlide.Flow
{
    public interface IModelBuilder
    {
        Model Configure<T>(
            IEnumerable<string> partsFiles,
            IEnumerable<string> tunnelsFiles,
            IEnumerable<string> settingsFiles) where T : IConfigurationEntryProvider, new();
    }
}