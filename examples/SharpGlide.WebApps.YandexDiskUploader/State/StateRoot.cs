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
            WorkingFolder = Environment.CurrentDirectory;
            SecuritySection = new SecurityState();
        }
        
        public string WorkingFolder { get; set; }

        public SecurityState SecuritySection { get; set; }

        public bool Authenticated => !string.IsNullOrWhiteSpace(SecuritySection.AccessToken);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}