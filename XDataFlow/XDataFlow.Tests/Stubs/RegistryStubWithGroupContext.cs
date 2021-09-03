using Moq;
using XDataFlow.Context;
using XDataFlow.Registry;

namespace XDataFlow.Tests.Stubs
{
    public class RegistryStubWithGroupContext : DefaultRegistry
    {
        public RegistryStubWithGroupContext()
        {
            this.Set<IMetaDataContext>(() => new Mock<IMetaDataContext>().Object);
            this.Set<IGroupContext>(()=>new GroupContext());
            this.Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            this.Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            this.Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
        }
    }
}