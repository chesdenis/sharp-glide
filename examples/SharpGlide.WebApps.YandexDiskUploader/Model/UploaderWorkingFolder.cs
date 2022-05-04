using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;

namespace SharpGlide.WebApps.YandexDiskUploader.Model
{
    public class UploaderWorkingFolder : ICloudFolderInformation
    {
        public string StatusCode { get; set; }
        public string Reason { get; set; }
        public string FullName { get; set; }
        public string CloudName { get; set; }
        public string Name { get; set; }
    }
}