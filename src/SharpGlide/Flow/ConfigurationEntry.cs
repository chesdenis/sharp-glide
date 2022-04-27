using System.Collections.Generic;

namespace SharpGlide.Flow
{
    public class ConfigurationEntry
    {
        public string AssemblyLocation { get; set; }
        public string FullName { get; set; }
        public string InstanceName { get; set; }
        public List<string> Dependencies { get; set; }
    }
}