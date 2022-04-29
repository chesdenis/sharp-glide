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
                context.GetInstance<ILoggerFactory>().CreateLogger<FolderContentsWalkTunnel>());
            
            this.For<ILogger<OAuthAuthorizeReadTunnel>>().Use(context =>
                context.GetInstance<ILoggerFactory>().CreateLogger<OAuthAuthorizeReadTunnel>());
            
            this.For<ILogger<CalculateTotalDirectorySizePart>>().Use(context =>
                context.GetInstance<ILoggerFactory>().CreateLogger<CalculateTotalDirectorySizePart>());
            
            this.For<ILogger<UploadFileToYandexPart>>().Use(context =>
                context.GetInstance<ILoggerFactory>().CreateLogger<UploadFileToYandexPart>());
            
            this.For<FolderContentsWalkTunnel>().Use<FolderContentsWalkTunnel>();
            this.For<OAuthAuthorizeReadTunnel>().Use<OAuthAuthorizeReadTunnel>();

            this.For<FlowModel>().Use<FlowModel>().Singleton();

            this.For<IReaderWithArg<OAuthAuthorizeReadTunnel.OAuthResponse, OAuthAuthorizeReadTunnel.OAuthRequest>>()
                .Use(context =>
                {
                    var flowModel = context.GetInstance<FlowModel>();
                    var tunnel = context
                        .GetInstance<OAuthAuthorizeReadTunnel>();
                    
                   return flowModel.BuildReader(tunnel);
                });

            this.For<IWalkerWithArg<FolderContentsWalkTunnel.FileMetadata, FolderContentsWalkTunnel.DirectoryMetadata>>()
                .Use(context =>
                {
                    var walkWithArgTunnel = context
                        .GetInstance<FolderContentsWalkTunnel>();
                    
                    return context.GetInstance<FlowModel>().BuildWalker(
                        walkWithArgTunnel);
                });
        }
    }
}