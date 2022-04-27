using Lamar;
using Microsoft.Extensions.Logging;
using Serilog;
using SharpGlide.FilesToCloudUploader.Parts;
using SharpGlide.FilesToCloudUploader.Tunnels;
using SharpGlide.Flow;
using SharpGlide.Tunnels.Readers.Interfaces;
using SharpGlide.Tunnels.Readers.Proxy;

namespace SharpGlide.FilesToCloudUploader
{
    public class AppRegistry : ServiceRegistry
    {
        public AppRegistry()
        {
            this.For<ILoggerFactory>().Use(context => LoggerFactory.Create(x => x.AddSerilog())).Singleton();

            this.For<ILogger<FolderContentsWalker>>().Use(context =>
                context.GetInstance<ILoggerFactory>()
                    .CreateLogger<FolderContentsWalker>());
            
            this.For<ILogger<CalculateTotalSizePart>>().Use(context =>
                context.GetInstance<ILoggerFactory>()
                    .CreateLogger<CalculateTotalSizePart>());
            
            this.For<FolderContentsWalker>().Use<FolderContentsWalker>();

            this.For<Model>().Use<Model>().Singleton();

            this.For<IWalkerByRequestProxy<FileAttributes, DirectoryRequest>>()
                .Use(context => context.GetInstance<Model>().GetProxy(
                    context.GetInstance<IWalkerByRequest<FileAttributes, DirectoryRequest>>()));
        }
    }
}