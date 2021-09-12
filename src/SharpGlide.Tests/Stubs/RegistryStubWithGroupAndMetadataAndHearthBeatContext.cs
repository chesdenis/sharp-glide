using SharpGlide.Context;
using SharpGlide.Providers;
using SharpGlide.Registry;
using Moq;
using SharpGlide.Context.HeartBeat;

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
            
            Set<IHeartBeatContext>(()=>new VectorHeartBeatContext(new Mock<IDateTimeProvider>().Object,
                Get<IConsumeMetrics>(),
                Get<IGroupContext>(),
                Get<IMetaDataContext>()
            ));
        }
    }
}