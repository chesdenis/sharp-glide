namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model
{
    public interface ICloudFileBytesRange
    {
        public string CloudName { get; set; }
        public byte[] Bytes { get; set; }
        public string Reason { get; set; }
    }
}