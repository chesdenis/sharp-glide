using DechFlow.Context;
using DechFlow.Registry;
using Moq;

namespace DechFlow.Tests.Stubs
{
    public class RegistryStubWithGroupContext : DefaultRegistry
    {
        public RegistryStubWithGroupContext()
        {
            Set<IMetaDataContext>(() => new Mock<IMetaDataContext>().Object);
            Set<IGroupContext>(()=>new GroupContext());
            Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
        }
    }
}