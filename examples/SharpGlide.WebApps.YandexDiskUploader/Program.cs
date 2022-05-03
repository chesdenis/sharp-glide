using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SharpGlide.WebApps.YandexDiskUploader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IConfiguration Configuration =
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("a67fee40-ee22-4eaf-ad79-bbbe3598c7d9")
                .Build();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x =>x.AddConfiguration(Configuration))
                .ConfigureHostConfiguration(x=>x.AddConfiguration(Configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var certPath = Configuration.GetValue<string>("Https:CertPath");
                    var certPassword = Configuration.GetValue<string>("Https:CertPassword");
                    webBuilder.UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 5050,
                            listenOptions =>
                            {
                                listenOptions.UseHttps(certPath,
                                    certPassword);
                            });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}