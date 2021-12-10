using System;

namespace SharpGlide.Providers
{
    public class GuidProvider : IGuidProvider
    {
        internal static IGuidProvider CustomProvider { get; set; }
        
        public Guid NewGuid() => CustomProvider?.NewGuid() ?? Guid.NewGuid();
    }
}