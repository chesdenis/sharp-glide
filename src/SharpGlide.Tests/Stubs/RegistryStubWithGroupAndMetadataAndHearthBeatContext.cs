using SharpGlide.Context;
using SharpGlide.Providers;
using SharpGlide.Registry;
using Moq;

namespace SharpGlide.Tests.Stubs
{
    public class RegistryStubWithGroupAndMetadataAndHearthBeatContext : DefaultRegistry
    {
        public RegistryStubWithGroupAndMetadataAndHearthBeatContext()
        {
            var metadataContext = new MetaDataContext();
            var groupContext = new GroupContext();
                
            Set<IMetaDataContext>(() => metadataContext);
            Set<IGroupContext>(() => groupContext);
          
            Set<IConsumeMetrics>(()=>new Mock<IConsumeMetrics>().Object);
            Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
            
            Set<IHeartBeatContext>(()=>new HeartBeatContext(new Mock<IDateTimeProvider>().Object,
                Get<IConsumeMetrics>(),
                Get<IGroupContext>(),
                Get<IMetaDataContext>()
            ));
        }
    }
}