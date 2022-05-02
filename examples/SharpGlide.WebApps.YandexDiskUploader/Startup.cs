using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpGlide.Cloud.Yandex.Readers.Authorization;
using SharpGlide.Cloud.Yandex.Readers.Profile;
using SharpGlide.Cloud.Yandex.Tunnels.Authorization;
using SharpGlide.Cloud.Yandex.Tunnels.Profile;
using SharpGlide.Flow;
using SharpGlide.WebApps.YandexDiskUploader.Config;
using SharpGlide.WebApps.YandexDiskUploader.Hubs;
using SharpGlide.WebApps.YandexDiskUploader.Parts;
using SharpGlide.WebApps.YandexDiskUploader.Service;


namespace SharpGlide.WebApps.YandexDiskUploader
{
    public class Startup
    {
        private static readonly HttpClient HttpClientInstance = new HttpClient();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            // put startup folder from server side to set it by default in page
            AppSetting.WorkingFolder = Environment.CurrentDirectory;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSignalR();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            
            services.AddSingleton(HttpClientInstance);
            services.AddSingleton(Configuration);
            services.AddSingleton<AppSetting>();
            
            services.AddSingleton<IBackgroundProcess, BackgroundProcess>();
            services.AddTransient<IUploadToCloudPart, UploadToCloudPart>();
            
            services.AddSingleton<FlowModel>();
            
            services.AddTransient<AuthorizeTokenUriReadTunnel>();
            services.AddTransient<IAuthorizeTokenUriReader>(
                provider =>
                {
                    var tunnel = provider
                        .GetService<AuthorizeTokenUriReadTunnel>();
            
                    return new AuthorizeTokenUriReader(tunnel.ReadSingleExpr.Compile());
                });
            
            services.AddTransient<ProfileReadTunnel>();
            services.AddTransient<IProfileReader>(provider =>
            {
                var tunnel = provider.GetService<ProfileReadTunnel>();
                return new ProfileReader(tunnel.ReadSingleExpr.Compile());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<RealtimeUpdatesHub>(RealtimeUpdatesHub.HubEndpoint);
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}