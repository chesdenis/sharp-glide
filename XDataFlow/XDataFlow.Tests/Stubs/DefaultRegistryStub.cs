using Moq;
using XDataFlow.Context;
using XDataFlow.Registry;

namespace XDataFlow.Tests.Stubs
{
    public class DefaultRegistryStub : DefaultRegistry
    {
        public DefaultRegistryStub()
        {
            this.Set<IMetaDataContext>(() => new Mock<IMetaDataContext>().Object);
            this.Set<IGroupContext>(()=>new Mock<IGroupContext>().Object);
            this.Set<IHeartBeatContext>(()=>new Mock<IHeartBeatContext>().Object);
            this.Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            this.Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
        }
    }
}