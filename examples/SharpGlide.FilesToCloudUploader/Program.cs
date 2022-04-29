using System;
using System.Threading;
using System.Threading.Tasks;
using Lamar;
using Microsoft.Extensions.Configuration;
using Serilog;
using SharpGlide.FilesToCloudUploader.Parts;

namespace SharpGlide.FilesToCloudUploader
{
    class Program
    {
        public static IConfiguration Configuration => new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets("a67fee40-ee22-4eaf-ad79-bbbe3598c7d9")
            .Build();

        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Started...");

            try
            {
                var appRegistry = new AppRegistry();
                appRegistry.For<IConfiguration>().Use(Configuration).Singleton();
                
                var appContainer = new Container(appRegistry);
                await appContainer.GetInstance<UploadFileToYandexPart>().ProcessAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            Log.Information("Finished");
        }
    }
}