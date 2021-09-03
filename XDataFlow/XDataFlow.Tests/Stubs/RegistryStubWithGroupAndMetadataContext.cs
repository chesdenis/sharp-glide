using Moq;
using XDataFlow.Context;
using XDataFlow.Registry;

namespace XDataFlow.Tests.Stubs
{
    public class RegistryStubWithGroupAndMetadataContext : DefaultRegistry
    {
        public RegistryStubWithGroupAndMetadataContext()
        {
            this.Set<IMetaDataContext>(() => new MetaDataContext());
            this.Set<IGroupContext>(()=>new GroupContext());
            this.Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            this.Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            this.Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
        }
    }
}