using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SharpGlide.WebApps.YandexDiskUploader.State
{
    public class StateRoot : BackgroundService, IStateRoot
    {
        public StateRoot()
        {
            LocalFolder = "/Folder1/Subfolder2/";
            CloudFolder = $"/UploadFolder_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";
            SecuritySection = new SecurityState();
        }
        
        public string LocalFolder { get; set; }
        public string CloudFolder { get; set; }

        public SecurityState SecuritySection { get; set; }

        public bool Authenticated => !string.IsNullOrWhiteSpace(SecuritySection.AccessToken);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}