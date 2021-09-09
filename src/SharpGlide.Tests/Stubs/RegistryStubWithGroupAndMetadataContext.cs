using SharpGlide.Context;
using SharpGlide.Registry;
using Moq;

namespace SharpGlide.Tests.Stubs
{
    public class RegistryStubWithGroupAndMetadataContext : DefaultRegistry
    {
        public RegistryStubWithGroupAndMetadataContext()
        {
            Set<IMetaDataContext>(() => new MetaDataContext());
            Set<IGroupContext>(()=>new GroupContext());
            Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
        }
    }
}