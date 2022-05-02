using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.IO.Model;

namespace SharpGlide.WebApps.YandexDiskUploader.Model
{
    public class UploaderWorkingFile : ICloudFileInformation
    {
        private readonly FsEntryInfo _fileSystemSection;

        public bool FileBytesReadCompleted { get; set; }
        
        public bool UploadUriObtained { get; set; }

        public bool UploadedToCloud { get; set; }

        public UploaderWorkingFile(FsEntryInfo fileSystemSection)
        {
            _fileSystemSection = fileSystemSection;
        }

        public string UploadUri { get; set; }
        public string StatusCode { get; set; }
        public string Reason { get; set; }
        public string FullName => _fileSystemSection.FullName;
        public string Name => _fileSystemSection.Name;
        public long Size => _fileSystemSection.Size;
    }
}