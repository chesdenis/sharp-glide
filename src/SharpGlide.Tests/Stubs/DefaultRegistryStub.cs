using SharpGlide.Context;
using SharpGlide.Registry;
using Moq;

namespace SharpGlide.Tests.Stubs
{
    public class DefaultRegistryStub : DefaultRegistry
    {
        public DefaultRegistryStub()
        {
            Set<IMetaDataContext>(() => new Mock<IMetaDataContext>().Object);
            Set<IGroupContext>(()=>new Mock<IGroupContext>().Object);
            Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
        }
    }
}