using Moq;
using XDataFlow.Context;
using XDataFlow.Providers;
using XDataFlow.Registry;

namespace XDataFlow.Tests.Stubs
{
    public class RegistryStubWithGroupAndMetadataAndHearthBeatContext : DefaultRegistry
    {
        public RegistryStubWithGroupAndMetadataAndHearthBeatContext()
        {
            var metadataContext = new MetaDataContext();
            var groupContext = new GroupContext();
                
            this.Set<IMetaDataContext>(() => metadataContext);
            this.Set<IGroupContext>(() => groupContext);
          
            this.Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            this.Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
            
            this.Set<IHeartBeatContext>(()=>new HeartBeatContext(new Mock<IDateTimeProvider>().Object,
                this.Get<IConsumeMetrics>(),
                this.Get<IGroupContext>(),
                this.Get<IMetaDataContext>()
            ));
        }
    }
}