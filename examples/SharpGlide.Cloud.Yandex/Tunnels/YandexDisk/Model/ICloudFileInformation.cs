namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model
{
    public interface ICloudFileInformation
    {
        public string UploadUri { get; set; }
        public string StatusCode { get; set; }
        public string Reason { get; set; }
        public string FullName { get; }
        public string Name { get; }
        public long Size { get; }
    }
}