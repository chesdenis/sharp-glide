using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.WebApps.YandexDiskUploader.Parts
{
    public class UploadToCloudPart : IUploadToCloudPart
    {
        public string Name { get; set; }
        
        public Task ProcessAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}