using Lamar;
using Microsoft.Extensions.Logging;
using Serilog;
using SharpGlide.FilesToCloudUploader.Model;
using SharpGlide.FilesToCloudUploader.Parts;
using SharpGlide.FilesToCloudUploader.Tunnels;
using SharpGlide.Flow;
using SharpGlide.Readers;
using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.FilesToCloudUploader
{
    public class AppRegistry : ServiceRegistry
    {
        public AppRegistry()
        {
            this.For<ILoggerFactory>().Use(context => LoggerFactory.Create(x => x.AddSerilog())).Singleton();

            this.For<ILogger<FolderContentsWalkTunnel>>().Use(context =>
                context.GetInstance<ILoggerFactory>()
                    .CreateLogger<FolderContentsWalkTunnel>());
            
            this.For<ILogger<CalculateTotalDirectorySizePart>>().Use(context =>
                context.GetInstance<ILoggerFactory>()
                    .CreateLogger<CalculateTotalDirectorySizePart>());
            
            this.For<FolderContentsWalkTunnel>().Use<FolderContentsWalkTunnel>();

            this.For<FlowModel>().Use<FlowModel>().Singleton();

            this.For<IWalkerWithArg<FolderContentsWalkTunnel.FileMetadata, FolderContentsWalkTunnel.DirectoryMetadata>>()
                .Use(context => context.GetInstance<FlowModel>().BuildWalker(
                    context.GetInstance<IWalkWithArgTunnel<FolderContentsWalkTunnel.FileMetadata, FolderContentsWalkTunnel.DirectoryMetadata>>()));
        }
    }
}