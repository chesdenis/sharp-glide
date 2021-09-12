using SharpGlide.Context;
using SharpGlide.Registry;
using Moq;
using SharpGlide.Providers;

namespace SharpGlide.Tests.Stubs
{
    public class DefaultRegistryStub : DefaultRegistry
    {
        public DefaultRegistryStub()
        {
            Set<IDateTimeProvider>(() => new Mock<IDateTimeProvider>().Object);
            Set<IMetaDataContext>(() => new Mock<IMetaDataContext>().Object);
            Set<IGroupContext>(()=>new Mock<IGroupContext>().Object);
            Set<ISettingsContext>(()=>new Mock<ISettingsContext>().Object);
        }
    }
}