using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.WebApps.YandexDiskUploader.Parts;

namespace SharpGlide.WebApps.YandexDiskUploader.Service
{
    public class BackgroundProcess : IBackgroundProcess
    {
        private readonly IUploadToCloudPart _uploadToCloudPart;

        private CancellationTokenSource _cancellationTokenSource;

        public bool Started { get; set; }

        private Task processTask;

        public BackgroundProcess(IUploadToCloudPart uploadToCloudPart)
        {
            _uploadToCloudPart = uploadToCloudPart;
        }

        public async Task StartAsync()
        {
            if (_cancellationTokenSource != null)
            {
                await StopAsync();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            
            processTask = Task.Run(async () =>
            {
                await _uploadToCloudPart.ProcessAsync(_cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);
            
            Started = true;
        }

        public Task StopAsync()
        {
            _cancellationTokenSource.Cancel();

            Started = false;
            return Task.CompletedTask;
        }
    }
}