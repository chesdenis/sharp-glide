using System.Threading.Tasks;

namespace SharpGlide.WebApps.YandexDiskUploader.Service
{
    public interface IBackgroundProcess
    {
        Task StartAsync();
        Task StopAsync();
    }
}