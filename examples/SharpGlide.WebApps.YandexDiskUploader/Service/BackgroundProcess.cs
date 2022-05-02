using System.Threading;
using System.Threading.Tasks;
using SharpGlide.WebApps.YandexDiskUploader.Parts;

namespace SharpGlide.WebApps.YandexDiskUploader.Service
{
    public class BackgroundProcess : IBackgroundProcess
    {
        private readonly IUploadToCloudPart _uploadToCloudPart;

        private CancellationTokenSource _cancellationTokenSource;

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

#pragma warning disable CS4014
            _uploadToCloudPart.ProcessAsync(_cancellationTokenSource.Token);
#pragma warning restore CS4014
        }

        public Task StopAsync()
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}